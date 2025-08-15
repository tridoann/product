namespace Product.Application.UnitOfWork;

public interface IBaseUnitOfWork<out TContext> : IDisposable
{
}