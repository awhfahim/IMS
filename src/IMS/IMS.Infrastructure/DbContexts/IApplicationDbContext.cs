using IMS.Domain.Categories;
using IMS.Domain.Products;
using IMS.Domain.Purchases;
using IMS.Domain.Sales;
using IMS.Domain.Stocks;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.DbContexts;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Purchase> Purchases { get; set; }
    DbSet<Stock> Stocks { get; set; }
    DbSet<Sale> Sales { get; set; }
}