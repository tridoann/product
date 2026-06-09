using MediatR;
using Microsoft.Extensions.Options;
using Product.Application.Models;
using Product.Application.Services;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Auth.LoginUser;

public class LoginUserRequestHandler(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher,
        IOptions<JwtOptions> jwtOptions,
        IUnitOfWork unitOfWork)
    : IRequestHandler<LoginUserRequest, LoginUserResponse>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken)
            ?? throw new KeyNotFoundException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account has been deactivated.");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new KeyNotFoundException("Invalid email or password.");

        user.LastLoginAt = DateTime.UtcNow;
        await unitOfWork.CommitAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new LoginUserResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Role = user.Role.ToString()
        };
    }
}
