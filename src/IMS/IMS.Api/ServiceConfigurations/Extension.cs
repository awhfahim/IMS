using IMS.Application.Contracts.DTOs;
using IMS.Domain.Products;
using Mapster;
using BindingFlags = System.Reflection.BindingFlags;

namespace IMS.Api.ServiceConfigurations;

public static class Extension
{
    public static void MapsterConfig(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        var constructorInfo = typeof(Product).GetConstructor(new[]
        {
            typeof(Guid), 
            typeof(string), 
            typeof(uint), 
            typeof(uint), 
            typeof(string), 
            typeof(string)
        });
        
        if (constructorInfo is not null)
        {
            config.NewConfig<ProductDto, Product>()
                .MapToConstructor(constructorInfo);
        }
    }
}