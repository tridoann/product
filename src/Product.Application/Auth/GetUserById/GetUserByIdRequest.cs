using MediatR;

namespace Product.Application.Auth.GetUserById;

public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
{
    public int UserId { get; set; }
}
