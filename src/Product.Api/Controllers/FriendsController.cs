using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Social.GetFriendRequests;
using Product.Application.Social.GetFriends;
using Product.Application.Social.RespondFriendRequest;
using Product.Application.Social.SendFriendRequest;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/friends")]
[Authorize]
public class FriendsController(IMediator mediator) : ControllerBase
{
    [HttpPost("request")]
    public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestRequest request, CancellationToken ct)
    {
        request.SenderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpPut("request/{id:int}")]
    public async Task<IActionResult> RespondRequest(int id, [FromBody] RespondFriendRequestRequest request, CancellationToken ct)
    {
        request.RequestId = id;
        request.ResponderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetFriends(CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetFriendsRequest { UserId = userId }, ct);
        return Ok(result);
    }

    [HttpGet("requests")]
    public async Task<IActionResult> GetFriendRequests(CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetFriendRequestsRequest { UserId = userId }, ct);
        return Ok(result);
    }
}
