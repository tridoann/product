using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Domain.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product.Domain.Entities.Product>
{
    public void Configure(EntityTypeBuilder<Product.Domain.Entities.Product> builder)
    {
        builder.HasKey(b => b.Id);
        builder.ToTable("Products");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(100);
        builder.Property(b => b.Description).HasMaxLength(500);
        builder.Property(b => b.Price).HasColumnType("numeric(18,2)");
        builder.Property(b => b.ImageUrl).HasMaxLength(500);

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(p => p.Seller)
            .WithMany()
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(b => b.CreatedAt).HasColumnType("timestamp without time zone");
        builder.Property(b => b.UpdatedAt).HasColumnType("timestamp without time zone");
    }
}