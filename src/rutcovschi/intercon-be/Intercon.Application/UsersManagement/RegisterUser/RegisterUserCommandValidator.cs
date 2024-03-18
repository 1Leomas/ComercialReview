using FluentValidation;
using Intercon.Application.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator(IUserRepository userRepository)
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

            RuleFor(x => x.Data.UserName)
                .MustAsync(userRepository.UserNameIsUniqueAsync!)
                .WithMessage("The username must be unique");
        });

        RuleFor(x => x.Data.Email)
            .MustAsync(userRepository.UserEmailExistsAsync)
            .WithMessage("The email must be unique");

        When(x => x.Data.Avatar is not null, () =>
        {
            RuleFor(x => x.Data.Avatar!.Data)
                .NotEmpty()
                .WithName(x => nameof(x.Data.Avatar)) //this doesnt work
                .WithMessage("Bad avatar");

        });
    }
}