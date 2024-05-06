using IMS.Domain.Products;
using IMS.Domain.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.Infrastructure.EntityTypeConfigurations;

public class SaleTypeConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.SaleDate).IsRequired();
        builder.Property(e => e.TotalPrice).IsRequired();
        builder.Property(e => e.Quantity).IsRequired();
        builder.Property(e => e.Discount);
        
        builder.HasIndex(e => e.ProductId);
        
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

    }
}