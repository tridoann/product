using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class CartItemRepository(ProductDbContext dbContext)
    : BaseRepository<CartItem, int>(dbContext), ICartItemRepository
{
    public Task<CartItem?> GetAsync(int cartId, int productId, CancellationToken ct = default)
        => _dbSet.FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId, ct);

    protected override IQueryable<CartItem> GetPagedCondition(string? searchQuery = null)
        => _dbSet;
}
