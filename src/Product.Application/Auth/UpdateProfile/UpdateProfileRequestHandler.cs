using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Auth.UpdateProfile;

public class UpdateProfileRequestHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProfileRequest, UpdateProfileResponse>
{
    public async Task<UpdateProfileResponse> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        user.DisplayName = request.DisplayName;
        user.Bio = request.Bio;
        if (request.AvatarUrl is not null)
            user.AvatarUrl = request.AvatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.CommitAsync(cancellationToken);

        return new UpdateProfileResponse
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Bio = user.Bio,
            AvatarUrl = user.AvatarUrl
        };
    }
}
