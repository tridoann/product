using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Admin.GetStats;
using Product.Application.Admin.GetUsers;
using Product.Application.Admin.SetUserActive;
using Product.Application.Commerce.Orders.GetOrders;
using Product.Application.Support.GetTickets;
using Product.Domain.Enums;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = "AdminOnly")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
        => Ok(await mediator.Send(new GetStatsRequest(), ct));

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 50, [FromQuery] string? search = null, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetUsersRequest { PageIndex = pageIndex, PageSize = pageSize, Search = search }, ct));

    [HttpPut("users/{id:int}/active")]
    public async Task<IActionResult> SetUserActive(int id, [FromBody] SetUserActiveRequest request, CancellationToken ct)
    {
        request.UserId = id;
        return Ok(await mediator.Send(request, ct));
    }

    [HttpGet("tickets")]
    public async Task<IActionResult> GetTickets([FromQuery] TicketStatus? status, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(new GetTicketsRequest
        {
            UserId = userId,
            IsAdmin = true,
            StatusFilter = status,
            PageIndex = pageIndex,
            PageSize = pageSize
        }, ct));
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => Ok(await mediator.Send(new GetOrdersRequest { IsAdmin = true, PageIndex = pageIndex, PageSize = pageSize }, ct));
}
