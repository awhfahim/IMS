using IMS.Application.Contracts.DTOs;

namespace IMS.Application.Contracts.Products;

public interface IProductManagementService
{
    Task AddProductAsync(ProductDto productDto);
}