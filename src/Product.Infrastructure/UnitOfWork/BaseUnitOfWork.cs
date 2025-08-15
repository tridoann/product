using Microsoft.EntityFrameworkCore;
using Product.Application.UnitOfWork;


namespace Product.Infrastructure.UnitOfWork;

public class BaseUnitOfWork<TContext> : IBaseUnitOfWork<TContext>
        , IDisposable
    where TContext : DbContext
{
    private readonly TContext _context;
    private bool _disposed = false;
    private Dictionary<Type, object> _repositories = [];


    public TContext DbContext => _context;


    public BaseUnitOfWork(TContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public int ExecuteSqlCommand(string sql, params object[] parameters)
        => _context.Database.ExecuteSqlRaw(sql, parameters);

    public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters)
            where TEntity : class
        => _context.Set<TEntity>().FromSqlRaw(sql, parameters);


    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
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