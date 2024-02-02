﻿namespace Intercon.Application.CustomExceptions;

public class ValidationException : Exception
{
    public ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }
}

public record ValidationError(string ProperyName, string ErrorMessage);