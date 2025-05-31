using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure.Database;

internal static class ModelBuilderSeeding
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product.Domain.Entities.Product>().HasData(
            new Product.Domain.Entities.Product
            {
                Id = 1,
                Name = "Sample Product",
                Description = "This is a sample product description.",
                Price = 19.99m,
                CreatedAt = new DateTime(2023, 1, 1),
                UpdatedAt = new DateTime(2023, 1, 1),
            }
        );
    }
}