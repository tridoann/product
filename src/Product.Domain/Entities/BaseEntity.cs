namespace Product.Domain.Entities;

public abstract class BaseEntity<TKey>
    where TKey : notnull
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Additional common properties can be added here
}