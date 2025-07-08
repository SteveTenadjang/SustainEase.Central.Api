using FluentValidation.Results;

namespace Central.Application.Exceptions;

public class ValidationException() : ApplicationException("One or more validation failures have occurred.")
{
    public Dictionary<string, string[]> Errors { get; } = new();

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(string propertyName, string errorMessage) : this()
    {
        Errors.Add(propertyName, [errorMessage]);
    }
}