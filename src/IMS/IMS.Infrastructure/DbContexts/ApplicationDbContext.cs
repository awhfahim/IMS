using IMS.Domain.Categories;
using IMS.Domain.Products;
using IMS.Domain.Purchases;
using IMS.Domain.Sales;
using IMS.Domain.Stocks;
using IMS.Infrastructure.EntityTypeConfigurations;
using IMS.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<AppUser,AppRole, Guid>(options), IApplicationDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Sale> Sales { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(e => e.RefreshToken).HasMaxLength(2000);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
        });
        
        builder.ApplyConfigurationsFromAssembly(typeof(ProductTypeConfiguration).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(SaleTypeConfiguration).Assembly);
    }

}