using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Application.Products.CreateProduct;
using Product.Application.Products.GetProduct;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController
        (IMediator mediator)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var request = new GetProductRequest()
        {
            Id = id
        };

        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Get([FromBody] CreateProductRequest request)
    {
        var response = await _mediator.Send(request);

        return Ok(response);
    }
}
