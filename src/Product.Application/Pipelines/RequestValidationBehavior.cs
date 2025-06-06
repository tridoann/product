using MediatR;
using Product.Application.Exceptions;

namespace Product.Application.Pipeline;

public class RequestValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<FluentValidation.IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<FluentValidation.IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new FluentValidation.ValidationContext<TRequest>(request);
        var tasks = _validators.Select(v => v.ValidateAsync(context));
        var validationResults = await Task.WhenAll(tasks);
        var failures = validationResults.SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToArray();

        if (failures.Length != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
