using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Products.CreateProduct;
using Product.Application.Products.GetProduct;
using Product.Application.Products.GetProducts;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController
        (IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductAsync(
        [FromRoute] int id)
    {
        var request = new GetProductRequest()
        {
            Id = id
        };

        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(
        [FromBody] CreateProductRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedListAsync(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? SearchQuery = null)
    {
        var request = new GetProductsRequest()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchQuery = SearchQuery,
        };

        var response = await _mediator.Send(request);

        return Ok(response);
    }
}
