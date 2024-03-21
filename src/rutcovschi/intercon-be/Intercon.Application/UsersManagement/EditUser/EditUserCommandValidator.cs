using FluentValidation;
using Intercon.Application.Abstractions;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .DependentRules(() =>
            {
                RuleFor(x => x.UserId)
                    .MustAsync(userRepository.UserExistsAsync)
                    .WithMessage("The user doesn't exists");
            });

        When(x => !string.IsNullOrEmpty(x.Data.FirstName), () =>
        {
            RuleFor(x => x.Data.FirstName)
                .MaximumLength(50)
                .WithName(x => nameof(x.Data.FirstName));
        });
        When(x => !string.IsNullOrEmpty(x.Data.LastName), () =>
        {
            RuleFor(x => x.Data.LastName)
                .MaximumLength(50)
                .WithName(x => nameof(x.Data.LastName));
        });

        When(x => !string.IsNullOrEmpty(x.Data.Email), () =>
        {
            RuleFor(x => x.Data.Email)
                .EmailAddress();

            RuleFor(x => x)
                .MustAsync(async (u, token)
                    => await userRepository.NewUserEmailIsFreeAsync(u.UserId, u.Data.Email!, token))
                .WithName(x => nameof(x.Data.Email))
                .WithMessage("The email must be unique");
        });

        When(x => !string.IsNullOrEmpty(x.Data.UserName), () =>
        {
            RuleFor(x => x.Data.UserName)
                .Length(2, 50)
                .WithName(x => nameof(x.Data.UserName));

            RuleFor(x => x)
                .MustAsync(async (u, token)
                    => await userRepository.NewUsernameIsFreeAsync(u.UserId, u.Data.UserName!, token))
                .WithName(x => nameof(x.Data.UserName))
                .WithMessage("The username must be unique");
        });
    }
}