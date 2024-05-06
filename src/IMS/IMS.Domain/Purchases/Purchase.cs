namespace IMS.Domain.Purchases;

public class Purchase : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid SupplierId { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public uint TotalPrice { get; set; }
    
}