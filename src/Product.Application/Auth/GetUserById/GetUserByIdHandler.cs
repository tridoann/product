using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Auth.GetUserById;

public class GetUserByIdHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");

        return new GetUserByIdResponse
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            CreatedAt = user.CreatedAt
        };
    }
}
