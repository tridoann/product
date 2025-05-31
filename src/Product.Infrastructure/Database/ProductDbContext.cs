using Microsoft.EntityFrameworkCore;
namespace Product.Infrastructure.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) 
    : DbContext(options)
{
    public DbSet<Product.Domain.Entities.Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();
    }
}