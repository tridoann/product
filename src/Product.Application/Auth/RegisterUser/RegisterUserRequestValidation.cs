using FluentValidation;

namespace Product.Application.Auth.RegisterUser;

public class RegisterUserRequestValidation : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username may only contain letters, digits, and underscores.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(100);

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
