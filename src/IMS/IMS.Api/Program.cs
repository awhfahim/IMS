using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using IMS.Api;
using IMS.Api.OptionsSetup;
using IMS.Api.Validators.DIExtensionsForFluentValidator;
using IMS.Application;
using IMS.Infrastructure;
using IMS.Infrastructure.DbContexts;
using IMS.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration().
    ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();
try
{
    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));
    
// Add services to the container.
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(b =>
    {
        b.RegisterModule(new ApplicationModule());
        b.RegisterModule(new InfrastructureModule());
        b.RegisterModule(new ApiModule());
    });
    
    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

//Configures Swagger
    builder.Services.AddSwaggerGen(option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
    
    var connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string is missing");
    
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;
    
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(connectionString,
            m => m.MigrationsAssembly(migrationAssembly)));
    
    //Use InMemory Database
    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //     options.UseInMemoryDatabase("appdb"));

    //Add JWT
    builder.Services.ConfigureOptions<JwtOptionsSetup>();
    builder.Services.AddJwtAuthentication();
    builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    
    //Add FluentValidation
    builder.Services.AddFluentValidationAutoValidation(s => s.DisableDataAnnotationsValidation = false)
        .AddFluentValidationClientsideAdapters();
    builder.Services.AddFluentValidationServices();
    

    //Add Api Endpoints
    //builder.Services.AddIdentityApiEndpoints<AppUser>()
    // .AddEntityFrameworkStores<ApplicationDbContext>();
    builder.Services.AddIdentity();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    //app.MapGroup("/account").MapIdentityApi<AppUser>();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
