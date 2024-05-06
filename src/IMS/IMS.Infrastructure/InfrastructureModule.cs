using Autofac;
using IMS.Application;
using IMS.Domain;
using IMS.Infrastructure.DbContexts;
using IMS.Infrastructure.Tokens;
using IMS.Infrastructure.UnitOfWorks;

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

        builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        
        base.Load(builder);
    }
}