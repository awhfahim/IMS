using Autofac;
using IMS.Application.Contracts.Products;
using IMS.Application.Products;

namespace IMS.Application;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ProductManagementService>().As<IProductManagementService>()
            .InstancePerLifetimeScope();
    }
}