namespace IMS.Domain.Products;

public class Product : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public uint BuyPrice { get; set; }
    public uint SellPrice { get; set; }
    public uint Stock { get; set; }
    
    public ProductCategory Category { get; set; }
}