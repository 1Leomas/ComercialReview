using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using MediatR;

namespace Intercon.Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new CustomExceptions.ValidationError
            (
                validationFailure.PropertyName.Replace("dto", ""),
                validationFailure.ErrorMessage
            ))
            .ToList();  

        if (errors.Any())
        {
            throw new CustomExceptions.ValidationException(errors);
        }

        var response = await next();

        return response;
    }
}