using MediatR;
using Product.Common.Models;

namespace Product.Application.Products.GetProducts;

public class GetProductsRequest : GetPagedListRequest, IRequest<GetProductsResponse>
{
}