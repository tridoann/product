using MediatR;

namespace Product.Application.Groups.GetGroups;

public class GetGroupsRequest : IRequest<GetGroupsResponse>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
}
