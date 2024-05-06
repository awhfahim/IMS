using Autofac;
using IMS.Application.Contracts.DTOs;
using IMS.Application.Contracts.Products;
using Mapster;

namespace IMS.Api.RequestHandlers;

public class ProductRequestHandler
{
    private IProductManagementService _productManagementService;
    public string Name { get; set; }
    public uint BuyPrice { get; set; }
    public uint SellPrice { get; set; }
    public string Brand { get; set; }
    public string Size { get; set; }
    public Guid CategoryId { get; set; }
    
    internal void Resolve(ILifetimeScope scope)
    {
        _productManagementService = scope.Resolve<IProductManagementService>();
    }


    public async Task AddProductAsync()
    {
        var productDto = await this.BuildAdapter().AdaptToTypeAsync<ProductDto>();
        await _productManagementService.AddProductAsync(productDto);
    }
}