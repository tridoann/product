using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class PostLikeRepository(ProductDbContext dbContext)
    : BaseRepository<PostLike, int>(dbContext), IPostLikeRepository
{
    public Task<PostLike?> GetAsync(int postId, int userId, CancellationToken ct = default)
        => _dbSet.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId, ct);

    protected override IQueryable<PostLike> GetPagedCondition(string? searchQuery = null)
        => _dbSet;
}
