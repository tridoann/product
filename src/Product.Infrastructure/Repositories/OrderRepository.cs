using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class OrderRepository(ProductDbContext dbContext)
    : BaseRepository<Order, int>(dbContext), IOrderRepository
{
    public async Task<PagedList<Order>> GetByUserAsync(int userId, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet.Where(o => o.BuyerId == userId).Include(o => o.Items).OrderByDescending(o => o.CreatedAt);
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<Order> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public async Task<PagedList<Order>> GetAllAsync(int pageIndex, int pageSize, CancellationToken ct = default)
    {
        var source = _dbSet.Include(o => o.Items).Include(o => o.Buyer).OrderByDescending(o => o.CreatedAt);
        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<Order> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public Task<int> CountAsync(CancellationToken ct = default) => _dbSet.CountAsync(ct);

    public Task<Order?> GetWithItemsAsync(int id, CancellationToken ct = default)
        => _dbSet.Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefaultAsync(o => o.Id == id, ct);

    protected override IQueryable<Order> GetPagedCondition(string? searchQuery = null)
        => _dbSet.Include(o => o.Items).OrderByDescending(o => o.CreatedAt);
}
