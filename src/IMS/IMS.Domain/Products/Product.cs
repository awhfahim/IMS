namespace IMS.Domain.Products;

public class Product(Guid categoryId, string name, uint buyPrice, uint sellPrice, string brand, string size)
    : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; } = categoryId;
    public string Name { get; set; } = name;
    public uint BuyPrice { get; set; } = buyPrice;
    public uint SellPrice { get; set; } = sellPrice;
    public string Brand { get; set; } = brand;
    public string Size { get; set; } = size;
}