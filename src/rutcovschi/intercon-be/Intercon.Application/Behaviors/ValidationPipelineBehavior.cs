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
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .Select(failure => new CustomExceptions.ValidationError
            (
                failure.PropertyName.Split(".").Last(),
                failure.ErrorMessage
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