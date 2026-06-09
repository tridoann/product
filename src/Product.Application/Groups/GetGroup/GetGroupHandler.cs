using MediatR;
using Product.Application.Exceptions;
using Product.Domain.Repositories;

namespace Product.Application.Groups.GetGroup;

public class GetGroupHandler(IGroupRepository groupRepository)
    : IRequestHandler<GetGroupRequest, GetGroupResponse>
{
    public async Task<GetGroupResponse> Handle(GetGroupRequest request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.GetWithMembersAsync(request.GroupId, cancellationToken)
            ?? throw new NotFoundException($"Group {request.GroupId} not found.");

        var membership = group.Members.FirstOrDefault(m => m.UserId == request.RequesterId);

        return new GetGroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            AvatarUrl = group.AvatarUrl,
            Privacy = group.Privacy,
            MemberCount = group.Members.Count,
            IsCurrentUserMember = membership != null,
            CurrentUserRole = membership?.Role,
            CreatedAt = group.CreatedAt,
            Members = group.Members.Select(m => new GroupMemberDto
            {
                UserId = m.UserId,
                Username = m.User.Username,
                DisplayName = m.User.DisplayName,
                AvatarUrl = m.User.AvatarUrl,
                Role = m.Role,
                JoinedAt = m.JoinedAt
            }).ToList()
        };
    }
}
