using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class FriendRequestRepository(ProductDbContext dbContext)
    : BaseRepository<FriendRequest, int>(dbContext), IFriendRequestRepository
{
    public Task<FriendRequest?> GetPendingAsync(int senderId, int receiverId, CancellationToken ct = default)
        => _dbSet.FirstOrDefaultAsync(r =>
            r.SenderId == senderId && r.ReceiverId == receiverId &&
            r.Status == FriendRequestStatus.Pending, ct);

    public async Task<IEnumerable<FriendRequest>> GetReceivedPendingAsync(int receiverId, CancellationToken ct = default)
        => await _dbSet
            .Include(r => r.Sender)
            .Where(r => r.ReceiverId == receiverId && r.Status == FriendRequestStatus.Pending)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    protected override IQueryable<FriendRequest> GetPagedCondition(string? searchQuery = null)
        => _dbSet.Include(r => r.Sender).Include(r => r.Receiver);
}
