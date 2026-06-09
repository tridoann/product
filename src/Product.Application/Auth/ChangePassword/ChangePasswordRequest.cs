using MediatR;

namespace Product.Application.Auth.ChangePassword;

public class ChangePasswordRequest : IRequest<ChangePasswordResponse>
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
