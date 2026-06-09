namespace Product.Application.Commerce.Cart.GetCart;

public class GetCartResponse
{
    public int CartId { get; set; }
    public List<CartItemDto> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
}

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ProductImageUrl { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
