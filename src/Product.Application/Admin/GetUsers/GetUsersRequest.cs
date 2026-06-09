using MediatR;

namespace Product.Application.Admin.GetUsers;

public class GetUsersRequest : IRequest<GetUsersResponse>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? Search { get; set; }
}
