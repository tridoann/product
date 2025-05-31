namespace Product.Application.Products.GetProduct;

public class GetProductResponse
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}