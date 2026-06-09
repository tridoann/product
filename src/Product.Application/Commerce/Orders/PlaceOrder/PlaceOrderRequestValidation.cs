using FluentValidation;

namespace Product.Application.Commerce.Orders.PlaceOrder;

public class PlaceOrderRequestValidation : AbstractValidator<PlaceOrderRequest>
{
    public PlaceOrderRequestValidation()
    {
        RuleFor(x => x.ShippingAddress).NotEmpty().MaximumLength(500);
    }
}
