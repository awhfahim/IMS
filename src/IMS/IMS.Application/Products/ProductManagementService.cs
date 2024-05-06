using IMS.Application.Contracts.DTOs;
using IMS.Application.Contracts.Products;
using IMS.Domain.Products;
using Mapster;
using MapsterMapper;

namespace IMS.Application.Products;

public class ProductManagementService(IApplicationUnitOfWork unitOfWork, IMapper mapper) : IProductManagementService
{
    public async Task AddProductAsync(ProductDto productDto)
    {
        var product = await productDto.BuildAdapter().AdaptToTypeAsync<Product>();

        await unitOfWork.Products.AddAsync(product);
        await unitOfWork.SaveAsync();
    }
}