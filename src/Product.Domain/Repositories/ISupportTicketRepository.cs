using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Enums;

namespace Product.Domain.Repositories;

public interface ISupportTicketRepository : IRepository<SupportTicket, int>
{
    Task<PagedList<SupportTicket>> GetByUserAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<PagedList<SupportTicket>> GetAllAsync(TicketStatus? status, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<SupportTicket?> GetWithRepliesAsync(int id, CancellationToken ct = default);
    Task<int> CountOpenAsync(CancellationToken ct = default);
}
