using FluentValidation;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(InterconDbContext context, UserManager<User> userManager)
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
                var user = await userManager.FindByNameAsync(userName!);
                return user is null ? true : false;
            }).WithMessage("The username must be unique");
        });

        RuleFor(x => x.Data.Email).MustAsync(async (email, _) =>
        {
            var user = await userManager.FindByEmailAsync(email);
            return user is null ? true : false;
        }).WithMessage("The email must be unique");

        When(x => x.Data.Avatar is not null, () =>
        {
            RuleFor(x => x.Data.Avatar!.Data)
                .NotEmpty()
                .WithName(x => nameof(x.Data.Avatar)) //this doesnt work
                .WithMessage("Bad avatar");

        });
    }
}