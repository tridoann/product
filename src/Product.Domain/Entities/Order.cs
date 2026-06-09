using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class Order : BaseEntity<int>
{
    public int BuyerId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;

    public User Buyer { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
}
