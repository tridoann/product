using FluentValidation;

namespace Product.Application.Support.ReplyToTicket;

public class ReplyToTicketRequestValidation : AbstractValidator<ReplyToTicketRequest>
{
    public ReplyToTicketRequestValidation()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}
