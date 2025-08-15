using Product.Application.UnitOfWork;

namespace Product.Infrastructure.UnitOfWork;

public class UnitOfWork(
    Product.Infrastructure.Database.ProductDbContext dbContext)
    : BaseUnitOfWork<Product.Infrastructure.Database.ProductDbContext>(dbContext),
    IUnitOfWork
{
}