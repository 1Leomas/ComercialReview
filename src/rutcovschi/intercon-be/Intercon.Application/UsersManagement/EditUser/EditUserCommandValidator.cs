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

            RuleFor(x => x.Data.Email)
                .MustAsync(userRepository.UserEmailExistsAsync)
                .WithMessage("The email must be unique")
                .WithName(x => nameof(x.Data.Email));
        });

        When(x => !string.IsNullOrEmpty(x.Data.UserName), () =>
        {
            RuleFor(x => x.Data.UserName)
                .Length(2, 50)
                .WithName(x => nameof(x.Data.UserName));

            RuleFor(x => x.Data.UserName)
                .MustAsync(userRepository.UserNameIsUniqueAsync!)
                .WithName(x => nameof(x.Data.UserName))
                .WithMessage("The username must be unique");
        });

        When(x => x.Data.Avatar is not null, () => 
        { 
            RuleFor(x => x.Data.Avatar!.Data)
            .NotEmpty()
            .WithName(x => nameof(x.Data.Avatar)) //this dont work
            .WithMessage("Bad avatar");

        });
    }
}