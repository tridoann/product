using FluentValidation;

namespace Product.Application.Social.CommentOnPost;

public class CommentOnPostRequestValidation : AbstractValidator<CommentOnPostRequest>
{
    public CommentOnPostRequestValidation()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
    }
}
