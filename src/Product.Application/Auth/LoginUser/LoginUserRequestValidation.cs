using FluentValidation;

namespace Product.Application.Auth.LoginUser;

public class LoginUserRequestValidation : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
