using MediatR;
using Product.Application.UnitOfWork;
using Product.Domain.Entities;
using Product.Domain.Repositories;

namespace Product.Application.Categories.CreateCategory;

public class CreateCategoryHandler(IProductCategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryRequest, CreateCategoryResponse>
{
    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        if (await categoryRepository.ExistsBySlugAsync(request.Slug, cancellationToken))
            throw new InvalidOperationException("A category with this slug already exists.");

        var category = new ProductCategory { Name = request.Name, Slug = request.Slug, ParentId = request.ParentId };
        await categoryRepository.CreateAsync(category, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new CreateCategoryResponse { Id = category.Id, Name = category.Name, Slug = category.Slug };
    }
}
