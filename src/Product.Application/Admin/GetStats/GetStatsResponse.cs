namespace Product.Application.Admin.GetStats;

public class GetStatsResponse
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalOrders { get; set; }
    public int OpenTickets { get; set; }
    public int TotalProducts { get; set; }
}
