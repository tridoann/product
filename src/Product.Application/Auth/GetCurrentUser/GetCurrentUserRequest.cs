using MediatR;

namespace Product.Application.Auth.GetCurrentUser;

public class GetCurrentUserRequest : IRequest<GetCurrentUserResponse>
{
    public int UserId { get; set; }
}
