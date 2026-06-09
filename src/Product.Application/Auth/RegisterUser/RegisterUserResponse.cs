namespace Product.Application.Auth.RegisterUser;

public class RegisterUserResponse
{
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
