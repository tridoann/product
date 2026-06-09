using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Commerce.Orders.GetOrders;
using Product.Application.Commerce.Orders.PlaceOrder;
using Product.Application.Commerce.Orders.UpdateOrderStatus;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(new GetOrdersRequest { UserId = userId, PageIndex = pageIndex, PageSize = pageSize }, ct));
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request, CancellationToken ct)
    {
        request.BuyerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(request, ct));
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        request.OrderId = id;
        return Ok(await mediator.Send(request, ct));
    }
}
