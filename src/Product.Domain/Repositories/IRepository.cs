using Product.Domain.Entities;

namespace Product.Domain.Repositories;

public interface IRepository<TEntity, TKey> 
    where TEntity : BaseEntity<TKey>
    where TKey : notnull
{
    IQueryable<TEntity> Get();
    Task<TEntity?> GetAsync(TKey key,
        CancellationToken cancellationToken = default);
    Task<TEntity?> CreateAsync(TEntity entity,
        CancellationToken cancellationToken = default);
}