using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Auth.GetUserById;
using Product.Application.Social.GetUserPosts;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken ct)
        => Ok(await mediator.Send(new GetUserByIdRequest { UserId = id }, ct));

    [HttpGet("{id:int}/posts")]
    public async Task<IActionResult> GetUserPosts(int id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 12, CancellationToken ct = default)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // Reuse GetFeed with authorId filter — use a dedicated query via repository
        var result = await mediator.Send(new GetUserPostsRequest { AuthorId = id, RequesterId = requesterId, PageIndex = pageIndex, PageSize = pageSize }, ct);
        return Ok(result);
    }
}
