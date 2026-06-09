using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Database.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.HasKey(f => f.Id);
        builder.ToTable("Friendships");

        builder.HasIndex(f => new { f.UserId, f.FriendId }).IsUnique();

        builder.HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Friend)
            .WithMany()
            .HasForeignKey(f => f.FriendId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
