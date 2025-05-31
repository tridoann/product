using Product.Infrastructure.Repositories;

namespace Product.Infrastructure.UnitOfWork;

public interface IUnitOfWork<out TContext>
{
    IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}