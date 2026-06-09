using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Commerce.Cart.AddToCart;
using Product.Application.Commerce.Cart.GetCart;
using Product.Application.Commerce.Cart.RemoveFromCart;
using System.Security.Claims;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(new GetCartRequest { UserId = userId }, ct));
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request, CancellationToken ct)
    {
        request.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(request, ct));
    }

    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> RemoveFromCart(int id, CancellationToken ct)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await mediator.Send(new RemoveFromCartRequest { CartItemId = id, UserId = userId }, ct));
    }
}
