namespace Product.Domain.Entities;

public class CartItem : BaseEntity<int>
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;

    public Cart Cart { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
