using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Database.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.Property(g => g.Name).HasMaxLength(100).IsRequired();
        builder.Property(g => g.Description).HasMaxLength(500);
        builder.Property(g => g.AvatarUrl).HasMaxLength(500);
        builder.Property(g => g.Privacy).HasConversion<int>();

        builder.HasOne(g => g.CreatedBy)
            .WithMany()
            .HasForeignKey(g => g.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
