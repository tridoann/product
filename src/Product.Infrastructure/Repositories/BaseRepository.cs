using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Infrastructure.Repositories;

public class BaseRepository<TEntity, TKey> 
    : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : notnull
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;


    public BaseRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> Get()
    {
        return _dbSet;
    }

    public Task<TEntity?> GetAsync(TKey key,
        CancellationToken cancellationToken = default)
    {
        return _dbSet.FirstOrDefaultAsync(
            e => e.Id.Equals(key), cancellationToken);
    }

    public async Task<TEntity?> CreateAsync(TEntity entity,
            CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var entry = await _dbSet.AddAsync(entity);
        return entry.Entity;
    }
}