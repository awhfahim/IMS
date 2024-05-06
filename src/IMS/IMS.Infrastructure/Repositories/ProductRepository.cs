using IMS.Domain.Products;
using IMS.Domain.Repositories;
using IMS.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Repositories;

public class ProductRepository(IApplicationDbContext context)
    : Repository<Product, Guid>(context as DbContext), IProductRepository
{
    
}