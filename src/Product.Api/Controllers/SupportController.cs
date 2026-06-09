using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Support.CreateTicket;
using Product.Application.Support.GetTicket;
using Product.Application.Support.GetTickets;
using Product.Application.Support.ReplyToTicket;
using Product.Application.Support.UpdateTicketStatus;
using Product.Domain.Enums;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/support/tickets")]
[Authorize]
public class SupportController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTickets(
        [FromQuery] TicketStatus? status,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.HasClaim("role", "Admin");
        return Ok(await mediator.Send(new GetTicketsRequest
        {
            UserId = userId,
            IsAdmin = isAdmin,
            StatusFilter = status,
            PageIndex = pageIndex,
            PageSize = pageSize
        }, ct));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTicket(int id, CancellationToken ct)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.HasClaim("role", "Admin");
        return Ok(await mediator.Send(new GetTicketRequest { TicketId = id, RequesterId = requesterId, IsAdmin = isAdmin }, ct));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketRequest request, CancellationToken ct)
    {
        request.SubmittedById = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(request, ct));
    }

    [HttpPost("{id:int}/replies")]
    public async Task<IActionResult> Reply(int id, [FromBody] ReplyToTicketRequest request, CancellationToken ct)
    {
        request.TicketId = id;
        request.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        request.IsAdminReply = User.HasClaim("role", "Admin");
        return Ok(await mediator.Send(request, ct));
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTicketStatusRequest request, CancellationToken ct)
    {
        request.TicketId = id;
        return Ok(await mediator.Send(request, ct));
    }
}
