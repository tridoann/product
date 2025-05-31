namespace Product.Domain.Entities;

public class Product: BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // Additional properties can be added as needed
}