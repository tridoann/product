using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Product.Domain.Repositories;

namespace Product.Application.Products.GetProduct;

public class GetProductRequestHandler(
        IProductRepository productRepository,
        IDistributedCache distributedCache)
    : IRequestHandler<GetProductRequest, GetProductResponse>
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var productInCache = await _distributedCache.GetStringAsync($"Product_{request.Id}", cancellationToken);
        if (!string.IsNullOrEmpty(productInCache))
        {
            // If not found in cache, fetch from database
            return System.Text.Json.JsonSerializer.Deserialize<GetProductResponse>(productInCache)
                ?? throw new InvalidOperationException("Deserialization failed.");
        }

        var product = await _productRepository.GetDetailAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

        var response = new GetProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            SellerId = product.SellerId,
            SellerDisplayName = product.Seller?.DisplayName,
            StockQuantity = product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive,
        };

        await _distributedCache.SetStringAsync(
            $"Product_{request.Id}",
            System.Text.Json.JsonSerializer.Serialize(response),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
            },
            cancellationToken);

        return response;
    }
}