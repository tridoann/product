using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Product.Infrastructure.Database;
using Product.Infrastructure.UnitOfWork;

namespace Product.Application.Products.GetProduct;

public class GetProductRequestHandler(
        IUnitOfWork<ProductDbContext> unitOfWork,
        IDistributedCache distributedCache) 
    : IRequestHandler<GetProductRequest, GetProductResponse>
{
    private readonly IUnitOfWork<ProductDbContext> _unitOfWork = unitOfWork;
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var productRepo = _unitOfWork.GetRepository<Domain.Entities.Product>();
        var product = await productRepo.Get()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken)
            ?? throw new KeyNotFoundException($"Product with ID {request.Id} not found.");

        return new GetProductResponse
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
}