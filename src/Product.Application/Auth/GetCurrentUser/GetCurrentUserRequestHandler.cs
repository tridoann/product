using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Auth.GetCurrentUser;

public class GetCurrentUserRequestHandler(IUserRepository userRepository)
    : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
{
    public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

        return new GetCurrentUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt
        };
    }
}
