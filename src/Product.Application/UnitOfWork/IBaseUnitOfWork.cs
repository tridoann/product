namespace Product.Application.UnitOfWork;

public interface IBaseUnitOfWork<out TContext> : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}