using Microsoft.EntityFrameworkCore;
using Product.Common.Models;
using System.Linq.Dynamic.Core;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : notnull
{
    protected readonly DbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;


    protected BaseRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetDbSet()
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

    protected abstract IQueryable<TEntity> GetPagedCondition(string? searchQuery = null);

    public virtual async Task<PagedList<TEntity>> ToPagedListAsync(
        int pageIndex,
        int pageSize,
        string? searchQuery = null,
        CancellationToken cancellationToken = default)
    {
        if (pageIndex < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageIndex), "Page index must be greater than or equal to 1.");
        }
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than or equal to 1.");
        }

        var source = GetPagedCondition(searchQuery);

        var totalItems = await source.CountAsync(cancellationToken);

        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<TEntity>
        {
            Items = items,
            TotalCount = totalItems,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public Type GetEntityType()
    {
        return typeof(TEntity);
    }
}