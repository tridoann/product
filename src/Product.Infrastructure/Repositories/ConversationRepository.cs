using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class ConversationRepository(ProductDbContext dbContext)
    : BaseRepository<Conversation, int>(dbContext), IConversationRepository
{
    public Task<Conversation?> GetDirectConversationAsync(int userId1, int userId2, CancellationToken ct = default)
        => _dbSet
            .Where(c => c.Type == ConversationType.Direct &&
                c.Participants.Any(p => p.UserId == userId1) &&
                c.Participants.Any(p => p.UserId == userId2))
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(ct);

    public Task<List<Conversation>> GetUserConversationsAsync(int userId, CancellationToken ct = default)
        => _dbSet
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Include(c => c.Participants).ThenInclude(p => p.User)
            .Include(c => c.Messages.Where(m => !m.IsDeleted).OrderByDescending(m => m.CreatedAt).Take(1))
            .OrderByDescending(c => c.Messages.Max(m => (DateTime?)m.CreatedAt))
            .ToListAsync(ct);

    public Task<Conversation?> GetWithParticipantsAsync(int id, CancellationToken ct = default)
        => _dbSet
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    protected override IQueryable<Conversation> GetPagedCondition(string? searchQuery = null)
        => _dbSet;
}
