using MediatR;

namespace Product.Application.Admin.SetUserActive;

public class SetUserActiveRequest : IRequest<SetUserActiveResponse>
{
    public int UserId { get; set; }
    public bool IsActive { get; set; }
}
