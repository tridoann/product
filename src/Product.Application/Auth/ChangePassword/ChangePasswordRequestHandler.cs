using MediatR;
using Product.Application.Services;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Auth.ChangePassword;

public class ChangePasswordRequestHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    : IRequestHandler<ChangePasswordRequest, ChangePasswordResponse>
{
    public async Task<ChangePasswordResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        if (!passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect.");

        user.PasswordHash = passwordHasher.Hash(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.CommitAsync(cancellationToken);

        return new ChangePasswordResponse();
    }
}
