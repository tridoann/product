using Product.Domain.Repositories;

namespace Product.Infrastructure.Repositories;

public class ProductRepository(
        Product.Infrastructure.Database.ProductDbContext dbContext)
    : BaseRepository<Product.Domain.Entities.Product, int>(dbContext),
    IProductRepository
{
}