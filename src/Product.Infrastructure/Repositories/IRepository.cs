namespace Product.Infrastructure.Repositories;

public interface IRepository<out TEntity> where TEntity : class
{
    IQueryable<TEntity> Get();
}