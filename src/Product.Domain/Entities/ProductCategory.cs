namespace Product.Domain.Entities;

public class ProductCategory : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }

    public ProductCategory? Parent { get; set; }
    public ICollection<ProductCategory> Children { get; set; } = [];
}
