using FluentValidation;

namespace Product.Application.Products.GetProducts;

public sealed class GetProductsRequestValidation : AbstractValidator<GetProductsRequest>
{
    public GetProductsRequestValidation()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Page index must be greater than or equal to 0.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than 0.");
    }
}