using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Repositories;

namespace Product.Application.Products.CreateProduct;

public class CreateProductRequestHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateProductRequest, CreateProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Domain.Entities.Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
        };

        await _productRepository.CreateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new CreateProductResponse
        {
            Id = product.Id,
            Name = product.Name
        };
    }
}