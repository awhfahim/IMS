namespace IMS.Domain.Stocks;

public class Stock : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
}