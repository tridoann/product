namespace Product.Application.Products.GetProduct;

public class GetProductResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? SellerId { get; set; }
    public string? SellerDisplayName { get; set; }
    public int StockQuantity { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
}