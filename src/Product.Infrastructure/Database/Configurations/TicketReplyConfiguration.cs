using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities;

namespace Product.Infrastructure.Database.Configurations;

public class TicketReplyConfiguration : IEntityTypeConfiguration<TicketReply>
{
    public void Configure(EntityTypeBuilder<TicketReply> builder)
    {
        builder.Property(r => r.Content).HasMaxLength(2000).IsRequired();

        builder.HasOne(r => r.Ticket)
            .WithMany(t => t.Replies)
            .HasForeignKey(r => r.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Author)
            .WithMany()
            .HasForeignKey(r => r.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
