using Product.Domain.Enums;

namespace Product.Application.Commerce.Orders.GetOrders;

public class GetOrdersResponse
{
    public List<OrderDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public int BuyerId { get; set; }
    public string? BuyerUsername { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
