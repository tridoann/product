namespace Product.Application.Auth.UpdateProfile;

public class UpdateProfileResponse
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}
