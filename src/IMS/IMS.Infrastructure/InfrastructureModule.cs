using Autofac;
using IMS.Infrastructure.DbContexts;
using IMS.Infrastructure.Tokens;

namespace IMS.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => c.Resolve<ApplicationDbContext>())
            .As<IApplicationDbContext>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<TokenService>().As<ITokenService>()
            .InstancePerLifetimeScope();
        base.Load(builder);
    }
}