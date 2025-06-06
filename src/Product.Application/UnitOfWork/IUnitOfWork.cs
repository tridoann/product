namespace Product.Application.UnitOfWork;

public interface IUnitOfWork
    : IBaseUnitOfWork<Infrastructure.Database.ProductDbContext>
{
}