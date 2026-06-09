using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Social.CommentOnPost;
using Product.Application.Social.CreatePost;
using Product.Application.Social.DeletePost;
using Product.Application.Social.GetFeed;
using Product.Application.Social.LikePost;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController(IMediator mediator) : ControllerBase
{
    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetFeedRequest { UserId = userId, PageIndex = pageIndex, PageSize = pageSize }, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request, CancellationToken ct)
    {
        request.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return CreatedAtAction(nameof(GetFeed), result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new DeletePostRequest { PostId = id, RequesterId = userId }, ct);
        return Ok(result);
    }

    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> LikePost(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new LikePostRequest { PostId = id, UserId = userId }, ct);
        return Ok(result);
    }

    [HttpPost("{id:int}/comments")]
    public async Task<IActionResult> CommentOnPost(int id, [FromBody] CommentOnPostRequest request, CancellationToken ct)
    {
        request.PostId = id;
        request.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }
}
