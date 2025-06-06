using MediatR;

namespace Product.Application.Products.CreateProduct;

public class CreateProductRequest : IRequest<CreateProductResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}