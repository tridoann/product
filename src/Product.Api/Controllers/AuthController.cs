using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Auth.ChangePassword;
using Product.Application.Auth.GetCurrentUser;
using Product.Application.Auth.LoginUser;
using Product.Application.Auth.RegisterUser;
using Product.Application.Auth.UpdateProfile;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMeAsync()
    {
        var userId = GetCurrentUserId();
        var response = await _mediator.Send(new GetCurrentUserRequest { UserId = userId });
        return Ok(response);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
    {
        request.UserId = GetCurrentUserId();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        request.UserId = GetCurrentUserId();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    private int GetCurrentUserId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("User ID not found in token.");
        return int.Parse(sub);
    }
}
