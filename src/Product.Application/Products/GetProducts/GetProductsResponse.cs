using Product.Application.Models;
using Product.Common.Models;

namespace Product.Application.Products.GetProducts;

public sealed class GetProductsResponse: PagedList<ProductDto>
{
}