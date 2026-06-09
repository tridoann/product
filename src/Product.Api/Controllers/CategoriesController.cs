using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Categories.CreateCategory;
using Product.Application.Categories.GetCategories;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
        => Ok(await mediator.Send(new GetCategoriesRequest(), ct));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
        => Ok(await mediator.Send(request, ct));
}
