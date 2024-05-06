using IMS.Domain.Categories;
using IMS.Domain.Products;
using IMS.Domain.Sales;
using IMS.Domain.Stocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.EntityTypeConfigurations;

public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.BuyPrice).IsRequired();
        builder.Property(e => e.SellPrice).IsRequired();
        builder.Property(e => e.Brand).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Size).HasMaxLength(100).IsRequired();

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.HasMany<Stock>()
            .WithOne()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasIndex(e => e.Name);
    }
}