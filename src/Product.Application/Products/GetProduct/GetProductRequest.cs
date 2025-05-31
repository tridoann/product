using MediatR;

namespace Product.Application.Products.GetProduct;

public class GetProductRequest: IRequest<GetProductResponse>
{
    public int Id { get; set; }
}