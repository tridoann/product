using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class MessageRepository(ProductDbContext dbContext)
    : BaseRepository<Message, int>(dbContext), IMessageRepository
{
    public async Task<PagedList<Message>> GetConversationMessagesAsync(int conversationId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet
            .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
            .Include(m => m.Sender)
            .OrderByDescending(m => m.CreatedAt);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<Message> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    protected override IQueryable<Message> GetPagedCondition(string? searchQuery = null)
        => _dbSet.Where(m => !m.IsDeleted).Include(m => m.Sender).OrderByDescending(m => m.CreatedAt);
}
