using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class CartRepository(ProductDbContext dbContext)
    : BaseRepository<Cart, int>(dbContext), ICartRepository
{
    public Task<Cart?> GetByUserIdAsync(int userId, CancellationToken ct = default)
        => _dbSet
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

    protected override IQueryable<Cart> GetPagedCondition(string? searchQuery = null)
        => _dbSet;
}
