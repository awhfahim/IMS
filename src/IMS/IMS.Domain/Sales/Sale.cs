namespace IMS.Domain.Sales;

public class Sale : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public DateOnly SaleDate { get; set; }
    public uint TotalPrice { get; set; }
    public uint Quantity { get; set; }
    public uint Discount { get; set; }
}