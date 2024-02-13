using FluentValidation;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.Data.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(x => nameof(x.Data.FirstName));
        RuleFor(x => x.Data.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithName(x => nameof(x.Data.LastName));
        RuleFor(x => x.Data.Email)
            .NotEmpty()
            .EmailAddress()
            .WithName(x => nameof(x.Data.Email));
        RuleFor(x => x.Data.Password)
            .NotEmpty()
            .Length(8, 50)
            .WithName(x => nameof(x.Data.Password));
        
        When(x => !x.Data.UserName.IsNullOrEmpty(), () =>
        {
            RuleFor(x => x.Data.UserName)
            .Length(2, 50)
            .WithName(x => nameof(x.Data.UserName));

            RuleFor(x => x.Data.UserName).MustAsync(async (userName, _) =>
            {
                return await context.Users.AllAsync(x => x.UserName != userName);
            }).WithMessage("The username must be unique");
        });

        RuleFor(x => x.Data.Email).MustAsync(async (email, _) =>
        {
            return await context.Users.AllAsync(x => x.Email != email);
        }).WithMessage("The email must be unique");
    }
}