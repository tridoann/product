using FluentValidation;

namespace Product.Application.Groups.CreateGroup;

public class CreateGroupRequestValidation : AbstractValidator<CreateGroupRequest>
{
    public CreateGroupRequestValidation()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
