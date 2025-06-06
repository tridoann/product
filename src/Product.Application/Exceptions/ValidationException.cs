using FluentValidation.Results;

namespace Product.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message)
        : base(message)
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ValidationException(ValidationFailure[] failures)
        : this("One or more validation failures have occurred.")
    {
        var propertyNames = failures
            .Select(e => e.PropertyName)
            .Distinct();

        foreach (var propertyName in propertyNames)
        {
            var propertyFailures = failures
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage)
                .ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public IDictionary<string, string[]> Failures { get; }
}
