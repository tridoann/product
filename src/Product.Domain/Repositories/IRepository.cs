using Product.Common.Models;
using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : notnull
{
    IQueryable<TEntity> GetDbSet();
    Task<TEntity?> GetAsync(TKey key,
        CancellationToken cancellationToken = default);
    Task<TEntity?> CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default);
    Task<PagedList<TEntity>> ToPagedListAsync(
        int pageIndex,
        int pageSize,
        string? searchQuery = null,
        CancellationToken cancellationToken = default);
    Type GetEntityType();
}