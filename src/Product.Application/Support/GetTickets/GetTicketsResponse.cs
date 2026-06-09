using Product.Domain.Enums;

namespace Product.Application.Support.GetTickets;

public class GetTicketsResponse
{
    public List<TicketSummaryDto> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TicketSummaryDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string SubmittedByUsername { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
