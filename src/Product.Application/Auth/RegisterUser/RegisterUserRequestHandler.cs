using MediatR;
using Product.Application.Services;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Auth.RegisterUser;

public class RegisterUserRequestHandler(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new InvalidOperationException($"Email '{request.Email}' is already registered.");

        if (await userRepository.ExistsByUsernameAsync(request.Username, cancellationToken))
            throw new InvalidOperationException($"Username '{request.Username}' is already taken.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            DisplayName = request.DisplayName,
        };

        await userRepository.CreateAsync(user, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(user);

        return new RegisterUserResponse
        {
            UserId = user.Id,
            Token = token,
            Username = user.Username,
            Email = user.Email
        };
    }
}
