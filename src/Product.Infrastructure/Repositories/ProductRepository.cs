using Microsoft.EntityFrameworkCore;
using Product.Common.Models;

namespace Product.Infrastructure.Repositories;

public class ProductRepository(
        Product.Infrastructure.Database.ProductDbContext dbContext)
    : BaseRepository<Product.Domain.Entities.Product, int>(dbContext),
    Product.Domain.Repositories.IProductRepository
{
    public async Task<Product.Domain.Entities.Product?> GetDetailAsync(int id, CancellationToken ct = default)
        => await _dbSet
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<PagedList<Product.Domain.Entities.Product>> GetPagedAsync(
        int? categoryId, string? search, int pageIndex, int pageSize, CancellationToken ct = default)
    {
        IQueryable<Product.Domain.Entities.Product> source = _dbSet
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Where(p => p.IsActive);

        if (categoryId.HasValue)
            source = source.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            source = source.Where(p => p.Name.Contains(search) || p.Description.Contains(search));

        source = source.OrderBy(p => p.Name);

        var total = await source.CountAsync(ct);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        return new PagedList<Product.Domain.Entities.Product> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };
    }

    public Task<int> CountActiveAsync(CancellationToken ct = default)
        => _dbSet.CountAsync(p => p.IsActive, ct);

    protected override IQueryable<Product.Domain.Entities.Product> GetPagedCondition(
        string? searchQuery = null)
    {
        var source = GetDbSet().Where(p => p.IsActive);
        if (!string.IsNullOrWhiteSpace(searchQuery))
            source = source.Where(p => p.Name.Contains(searchQuery) || p.Description.Contains(searchQuery));
        return source.OrderBy(p => p.Name);
    }
}