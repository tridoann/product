namespace Product.Domain.Repositories;

public interface IProductRepository
    : IRepository<Product.Domain.Entities.Product, int>
{
}