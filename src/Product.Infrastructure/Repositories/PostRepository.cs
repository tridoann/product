using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class PostRepository(ProductDbContext dbContext)
    : BaseRepository<Post, int>(dbContext), IPostRepository
{
    public async Task<PagedList<Post>> GetFeedAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var friendIds = await _dbContext.Set<Friendship>()
            .Where(f => f.UserId == userId || f.FriendId == userId)
            .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
            .ToListAsync(ct);

        var source = _dbSet
            .Where(p => !p.IsDeleted && p.GroupId == null &&
                (p.AuthorId == userId || friendIds.Contains(p.AuthorId)))
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Author)
            .OrderByDescending(p => p.CreatedAt);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<Post>
        {
            Items = items,
            TotalCount = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<PagedList<Post>> GetGroupPostsAsync(int groupId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet
            .Where(p => !p.IsDeleted && p.GroupId == groupId)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Author)
            .OrderByDescending(p => p.CreatedAt);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<Post>
        {
            Items = items,
            TotalCount = total,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<PagedList<Post>> GetUserPostsAsync(int authorId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet
            .Where(p => !p.IsDeleted && p.AuthorId == authorId)
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Author)
            .OrderByDescending(p => p.CreatedAt);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedList<Post> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public Task<int> CountUserPostsAsync(int authorId, CancellationToken ct = default)
        => _dbSet.CountAsync(p => !p.IsDeleted && p.AuthorId == authorId, ct);

    protected override IQueryable<Post> GetPagedCondition(string? searchQuery = null)
    {
        IQueryable<Post> source = _dbSet.Where(p => !p.IsDeleted).Include(p => p.Author);
        if (!string.IsNullOrWhiteSpace(searchQuery))
            source = source.Where(p => p.Content.Contains(searchQuery));
        return source.OrderByDescending(p => p.CreatedAt);
    }
}
