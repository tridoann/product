using MediatR;
using Product.Domain.Repositories;

namespace Product.Application.Categories.GetCategories;

public class GetCategoriesHandler(IProductCategoryRepository categoryRepository)
    : IRequestHandler<GetCategoriesRequest, GetCategoriesResponse>
{
    public async Task<GetCategoriesResponse> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var all = await categoryRepository.GetAllAsync(cancellationToken);
        return new GetCategoriesResponse
        {
            Items = all.Where(c => c.ParentId == null).Select(Map).ToList()
        };
    }

    private static CategoryDto Map(Domain.Entities.ProductCategory c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Slug = c.Slug,
        ParentId = c.ParentId,
        Children = c.Children.Select(Map).ToList()
    };
}
