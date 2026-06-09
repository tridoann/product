using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IProductCategoryRepository : IRepository<ProductCategory, int>
{
    Task<List<ProductCategory>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsBySlugAsync(string slug, CancellationToken ct = default);
}
