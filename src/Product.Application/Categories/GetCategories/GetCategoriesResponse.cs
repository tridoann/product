namespace Product.Application.Categories.GetCategories;

public class GetCategoriesResponse
{
    public List<CategoryDto> Items { get; set; } = [];
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public List<CategoryDto> Children { get; set; } = [];
}
