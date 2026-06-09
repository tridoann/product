using MediatR;

namespace Product.Application.Categories.CreateCategory;

public class CreateCategoryRequest : IRequest<CreateCategoryResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}
