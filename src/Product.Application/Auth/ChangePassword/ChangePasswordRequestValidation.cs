using FluentValidation;

namespace Product.Application.Auth.ChangePassword;

public class ChangePasswordRequestValidation : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidation()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).MaximumLength(100);
    }
}
