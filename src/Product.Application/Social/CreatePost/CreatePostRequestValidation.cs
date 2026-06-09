using FluentValidation;

namespace Product.Application.Social.CreatePost;

public class CreatePostRequestValidation : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidation()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.MediaUrl).MaximumLength(500);
    }
}
