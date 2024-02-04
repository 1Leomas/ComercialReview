using FluentValidation;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(50);
        RuleFor(x => x.UserName)
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(x => x.UserName).MustAsync(async (userName, _) =>
        {
            return await context.Users.AllAsync(x => x.UserName != userName);
        }).WithMessage("The username must be unique");

        RuleFor(x => x.Email).MustAsync(async (email, _) =>
        {
            return await context.Users.AllAsync(x => x.Email != email);
        }).WithMessage("The email must be unique");
    }
}