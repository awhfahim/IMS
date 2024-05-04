using FluentValidation;
using IMS.Api.RequestHandlers;

namespace IMS.Api.Validators.DIExtensionsForFluentValidator;

public static class FluentValidationServices
{
    public static void AddFluentValidationServices(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegistrationRequestHandler>, RegistrationRequestValidator>();
        //services.AddValidatorsFromAssemblyContaining<RegistrationRequestValidator>();
    }
}