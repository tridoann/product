using Product.Common.Models;

namespace Product.Domain.Repositories;

public interface IProductRepository
    : IRepository<Product.Domain.Entities.Product, int>
{
    Task<Product.Domain.Entities.Product?> GetDetailAsync(int id, CancellationToken ct = default);
    Task<PagedList<Product.Domain.Entities.Product>> GetPagedAsync(int? categoryId, string? search, int pageIndex, int pageSize, CancellationToken ct = default);
    Task<int> CountActiveAsync(CancellationToken ct = default);
}