using FluentValidation;

namespace Product.Application.Auth.UpdateProfile;

public class UpdateProfileRequestValidation : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidation()
    {
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Bio).MaximumLength(500);
        RuleFor(x => x.AvatarUrl).MaximumLength(500);
    }
}
