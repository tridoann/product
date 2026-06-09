using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class CommentRepository(ProductDbContext dbContext)
    : BaseRepository<Comment, int>(dbContext), ICommentRepository
{
    protected override IQueryable<Comment> GetPagedCondition(string? searchQuery = null)
        => _dbSet.Where(c => !c.IsDeleted).OrderByDescending(c => c.CreatedAt);
}
