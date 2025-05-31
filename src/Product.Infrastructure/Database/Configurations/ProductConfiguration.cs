using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Product.Infrastructure.Domain.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product.Domain.Entities.Product>
{
    public void Configure(EntityTypeBuilder<Product.Domain.Entities.Product> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Products");

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .HasMaxLength(500);
    }
}