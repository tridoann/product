using FluentValidation;

namespace Product.Application.Support.CreateTicket;

public class CreateTicketRequestValidation : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidation()
    {
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
    }
}
