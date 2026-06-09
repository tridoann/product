using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Groups.CreateGroup;
using Product.Application.Groups.GetGroup;
using Product.Application.Groups.GetGroups;
using Product.Application.Groups.JoinGroup;
using Product.Application.Groups.LeaveGroup;
using Product.Application.Groups.UpdateGroupMemberRole;
using Product.Application.Social.GetGroupPosts;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/groups")]
[Authorize]
public class GroupsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGroups([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetGroupsRequest { PageIndex = pageIndex, PageSize = pageSize, Search = search }, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGroup(int id, CancellationToken ct)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetGroupRequest { GroupId = id, RequesterId = requesterId }, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request, CancellationToken ct)
    {
        request.CreatedById = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return CreatedAtAction(nameof(GetGroups), result);
    }

    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> JoinGroup(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new JoinGroupRequest { GroupId = id, UserId = userId }, ct);
        return Ok(result);
    }

    [HttpDelete("{id:int}/leave")]
    public async Task<IActionResult> LeaveGroup(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new LeaveGroupRequest { GroupId = id, UserId = userId }, ct);
        return Ok(result);
    }

    [HttpPut("{id:int}/members/{targetUserId:int}/role")]
    public async Task<IActionResult> UpdateMemberRole(int id, int targetUserId, [FromBody] UpdateGroupMemberRoleRequest request, CancellationToken ct)
    {
        request.GroupId = id;
        request.TargetUserId = targetUserId;
        request.RequesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}/posts")]
    public async Task<IActionResult> GetGroupPosts(int id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetGroupPostsRequest { GroupId = id, PageIndex = pageIndex, PageSize = pageSize }, ct);
        return Ok(result);
    }
}
