using MediatR;

namespace Product.Application.Auth.UpdateProfile;

public class UpdateProfileRequest : IRequest<UpdateProfileResponse>
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}
