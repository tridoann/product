namespace Product.Domain.Entities;

public class Product : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public int? SellerId { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ProductCategory? Category { get; set; }
    public User? Seller { get; set; }
}