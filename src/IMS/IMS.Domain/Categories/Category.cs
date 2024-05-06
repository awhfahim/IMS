namespace IMS.Domain.Categories;

public class Category : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}