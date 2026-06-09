using MediatR;
using Product.Application.Models;
using Product.Domain.Repositories;

namespace Product.Application.Products.GetProducts;

public class GetProductsRequestHandler : IRequestHandler<GetProductsRequest, GetProductsResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsRequestHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductsResponse> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetPagedAsync(
                request.CategoryId,
                request.SearchQuery,
                request.PageIndex,
                request.PageSize,
                cancellationToken);

        return new GetProductsResponse()
        {
            Items = [.. products.Items.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name,
                    SellerId = p.SellerId,
                    SellerDisplayName = p.Seller?.DisplayName,
                    StockQuantity = p.StockQuantity,
                    ImageUrl = p.ImageUrl,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })],
            TotalCount = products.TotalCount,
            PageIndex = products.PageIndex,
            PageSize = products.PageSize
        };
    }
}