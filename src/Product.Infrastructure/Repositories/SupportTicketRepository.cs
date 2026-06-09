using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class SupportTicketRepository(ProductDbContext dbContext)
    : BaseRepository<SupportTicket, int>(dbContext), ISupportTicketRepository
{
    public async Task<PagedList<SupportTicket>> GetByUserAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet
            .Include(t => t.SubmittedBy)
            .Where(t => t.SubmittedById == userId)
            .OrderByDescending(t => t.CreatedAt);
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<SupportTicket> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public async Task<PagedList<SupportTicket>> GetAllAsync(TicketStatus? status, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        IQueryable<SupportTicket> source = _dbSet.Include(t => t.SubmittedBy);
        if (status.HasValue)
            source = source.Where(t => t.Status == status.Value);
        source = source.OrderByDescending(t => t.CreatedAt);
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<SupportTicket> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public Task<SupportTicket?> GetWithRepliesAsync(int id, CancellationToken ct = default)
        => _dbSet
            .Include(t => t.SubmittedBy)
            .Include(t => t.AssignedTo)
            .Include(t => t.Replies).ThenInclude(r => r.Author)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<int> CountOpenAsync(CancellationToken ct = default)
        => _dbSet.CountAsync(t => t.Status == TicketStatus.Open, ct);

    protected override IQueryable<SupportTicket> GetPagedCondition(string? searchQuery = null)
        => _dbSet.OrderByDescending(t => t.CreatedAt);
}
