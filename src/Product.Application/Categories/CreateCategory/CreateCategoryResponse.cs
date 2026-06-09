namespace Product.Application.Categories.CreateCategory;

public class CreateCategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
