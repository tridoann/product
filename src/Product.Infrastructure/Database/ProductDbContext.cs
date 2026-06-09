using Microsoft.EntityFrameworkCore;
using Product.Domain.Entities;

namespace Product.Infrastructure.Database;

public class ProductDbContext(DbContextOptions<ProductDbContext> options)
    : DbContext(options)
{
    public DbSet<Product.Domain.Entities.Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Friendship> Friendships { get; set; } = null!;
    public DbSet<FriendRequest> FriendRequests { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostLike> PostLikes { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<GroupMember> GroupMembers { get; set; } = null!;
    public DbSet<Conversation> Conversations { get; set; } = null!;
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<SupportTicket> SupportTickets { get; set; } = null!;
    public DbSet<TicketReply> TicketReplies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Seed();

        // Apply 'timestamp without time zone' globally to all DateTime columns
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                property.SetColumnType("timestamp without time zone");
            }
        }
    }
}