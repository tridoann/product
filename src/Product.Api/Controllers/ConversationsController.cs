using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Messaging.GetConversations;
using Product.Application.Messaging.GetMessages;
using Product.Application.Messaging.GetOrCreateDirectConversation;
using Product.Application.Messaging.MarkConversationRead;
using Product.Application.Messaging.SendMessage;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetConversations(CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetConversationsRequest { UserId = userId }, ct);
        return Ok(result);
    }

    [HttpPost("direct")]
    public async Task<IActionResult> GetOrCreateDirect([FromBody] GetOrCreateDirectConversationRequest request, CancellationToken ct)
    {
        request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(request, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}/messages")]
    public async Task<IActionResult> GetMessages(int id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetMessagesRequest { ConversationId = id, UserId = userId, PageIndex = pageIndex, PageSize = pageSize }, ct);
        return Ok(result);
    }

    [HttpPost("{id:int}/messages")]
    public async Task<IActionResult> SendMessage(int id, [FromBody] SendMessageRequest request, CancellationToken ct)
    {
        request.ConversationId = id;
        request.SenderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(request, ct));
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new MarkConversationReadRequest { ConversationId = id, UserId = userId }, ct);
        return Ok(result);
    }
}
