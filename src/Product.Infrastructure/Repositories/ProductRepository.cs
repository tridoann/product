namespace Product.Infrastructure.Repositories;

public class ProductRepository(
        Product.Infrastructure.Database.ProductDbContext dbContext)
    : BaseRepository<Product.Domain.Entities.Product, int>(dbContext),
    Product.Domain.Repositories.IProductRepository
{
    protected override IQueryable<Product.Domain.Entities.Product> GetPagedCondition(
        string? searchQuery = null)
    {
        var source = GetDbSet();
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            source = source.Where(p => p.Name.Contains(searchQuery)
                || p.Description.Contains(searchQuery));
        }
        return source.OrderBy(p => p.Name);
    }
}