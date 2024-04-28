namespace Intercon.Application.CustomExceptions;

public class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }

    public ValidationException(string propertyName, string message)
        : base("Validation failed")
    {
        Errors = new ValidationError[]
        {
            new ValidationError(propertyName, message)
        };
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }
}

public record ValidationError(string PropertyName, string ErrorMessage);