using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Product.Infrastructure.Repositories;

namespace Product.Infrastructure.UnitOfWork;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
{
    private readonly TContext _context;
    private bool _disposed = false;
    private Dictionary<Type, object> _repositories = [];


    public TContext DbContext => _context;


    public UnitOfWork(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
    {
        _repositories ??= [];

        if (hasCustomRepository)
        {
            var customRepo = _context.GetService<IRepository<TEntity>>();
            if (customRepo is not null)
            {
                return customRepo;
            }
        }

        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            _repositories[type] = new BaseRepository<TEntity>(_context);
        }

        return (IRepository<TEntity>)_repositories[type];
    }

    public int ExecuteSqlCommand(string sql, params object[] parameters)
        => _context.Database.ExecuteSqlRaw(sql, parameters);

    public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters)
            where TEntity : class
        => _context.Set<TEntity>().FromSqlRaw(sql, parameters);


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // clear repositories
            _repositories?.Clear();

            // dispose the db context.
            _context.Dispose();
        }

        _disposed = true;
    }
}