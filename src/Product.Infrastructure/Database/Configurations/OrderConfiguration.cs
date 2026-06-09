using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Database.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.TotalAmount).HasColumnType("numeric(18,2)");
        builder.Property(o => o.ShippingAddress).HasMaxLength(500).IsRequired();
        builder.Property(o => o.Status).HasConversion<int>();

        builder.HasOne(o => o.Buyer)
            .WithMany()
            .HasForeignKey(o => o.BuyerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
