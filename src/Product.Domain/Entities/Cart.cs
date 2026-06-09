namespace Product.Domain.Entities;

public class Cart : BaseEntity<int>
{
    public int UserId { get; set; }

    public User User { get; set; } = null!;
    public ICollection<CartItem> Items { get; set; } = [];
}
