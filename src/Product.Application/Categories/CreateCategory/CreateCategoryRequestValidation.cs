using FluentValidation;

namespace Product.Application.Categories.CreateCategory;

public class CreateCategoryRequestValidation : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidation()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(100)
            .Matches("^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric with hyphens.");
    }
}
