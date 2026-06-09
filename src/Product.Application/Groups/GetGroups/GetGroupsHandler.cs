using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Groups.GetGroups;

public class GetGroupsHandler(IGroupRepository groupRepository)
    : IRequestHandler<GetGroupsRequest, GetGroupsResponse>
{
    public async Task<GetGroupsResponse> Handle(GetGroupsRequest request, CancellationToken cancellationToken)
    {
        var paged = await groupRepository.GetPublicGroupsAsync(request.PageIndex, request.PageSize, request.Search, cancellationToken);

        return new GetGroupsResponse
        {
            TotalCount = paged.TotalCount,
            PageIndex = paged.PageIndex,
            PageSize = paged.PageSize,
            Items = paged.Items.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                AvatarUrl = g.AvatarUrl,
                Privacy = g.Privacy,
                MemberCount = g.Members.Count,
                CreatedAt = g.CreatedAt
            }).ToList()
        };
    }
}
