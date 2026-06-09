using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;
using Product.Infrastructure.Database;

namespace Product.Infrastructure.Repositories;

public class ProductCategoryRepository(ProductDbContext dbContext)
    : BaseRepository<ProductCategory, int>(dbContext), IProductCategoryRepository
{
    public Task<List<ProductCategory>> GetAllAsync(CancellationToken ct = default)
        => _dbSet.Include(c => c.Children).OrderBy(c => c.Name).ToListAsync(ct);

    public Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default)
        => _dbSet.AnyAsync(c => c.Slug == slug, ct);

    protected override IQueryable<ProductCategory> GetPagedCondition(string? searchQuery = null)
        => _dbSet.OrderBy(c => c.Name);
}
