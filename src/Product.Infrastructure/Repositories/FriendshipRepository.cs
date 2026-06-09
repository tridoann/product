using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class FriendshipRepository(ProductDbContext dbContext)
    : BaseRepository<Friendship, int>(dbContext), IFriendshipRepository
{
    public Task<bool> AreFriendsAsync(int userId, int friendId, CancellationToken ct = default)
        => _dbSet.AnyAsync(f =>
            (f.UserId == userId && f.FriendId == friendId) ||
            (f.UserId == friendId && f.FriendId == userId), ct);

    public async Task<IEnumerable<Friendship>> GetFriendshipsAsync(int userId, CancellationToken ct = default)
        => await _dbSet
            .Include(f => f.User)
            .Include(f => f.Friend)
            .Where(f => f.UserId == userId || f.FriendId == userId)
            .ToListAsync(ct);

    protected override IQueryable<Friendship> GetPagedCondition(string? searchQuery = null)
        => _dbSet.Include(f => f.User).Include(f => f.Friend);
}
