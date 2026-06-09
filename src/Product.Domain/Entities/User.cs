using Product.Domain.Enums;

namespace Product.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public UserRole Role { get; set; } = UserRole.Member;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
}
