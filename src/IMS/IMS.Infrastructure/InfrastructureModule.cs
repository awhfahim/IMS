using Autofac;
using IMS.Infrastructure.Membership.Tokens;

namespace IMS.Infrastructure;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<TokenService>().As<ITokenService>()
            .InstancePerLifetimeScope();
        base.Load(builder);
    }
}