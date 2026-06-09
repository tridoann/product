using FluentValidation;

namespace Product.Application.Messaging.SendMessage;

public class SendMessageRequestValidation : AbstractValidator<SendMessageRequest>
{
    public SendMessageRequestValidation()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(2000);
    }
}
