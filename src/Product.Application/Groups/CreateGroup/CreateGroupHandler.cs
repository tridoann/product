using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Enums;
using Product.Domain.Repositories;

namespace Product.Application.Groups.CreateGroup;

public class CreateGroupHandler(IGroupRepository groupRepository, IGroupMemberRepository memberRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGroupRequest, CreateGroupResponse>
{
    public async Task<CreateGroupResponse> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var group = new Group
        {
            Name = request.Name,
            Description = request.Description,
            Privacy = request.Privacy,
            CreatedById = request.CreatedById
        };

        await groupRepository.CreateAsync(group, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // Creator becomes admin
        await memberRepository.CreateAsync(new GroupMember
        {
            GroupId = group.Id,
            UserId = request.CreatedById,
            Role = GroupMemberRole.Admin,
            JoinedAt = DateTime.UtcNow
        }, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateGroupResponse { Id = group.Id, Name = group.Name, Privacy = group.Privacy, CreatedAt = group.CreatedAt };
    }
}
