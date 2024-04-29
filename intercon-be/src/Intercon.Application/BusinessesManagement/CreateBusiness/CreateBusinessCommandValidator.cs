using FluentValidation;
using Intercon.Application.Abstractions;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator(IBusinessRepository businessRepository, IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .DependentRules(() =>
            {
                RuleFor(x => x.UserId)
                    .MustAsync(userRepository.UserExistsAsync)
                    .WithMessage("The user doesn't exists");
            });

        RuleFor(x => x.UserId)
            .MustAsync(businessRepository.UserHasBusinessAsync)
            .WithMessage("The user already owns a business");

        RuleFor(x => x.Data.Title)
            .NotEmpty()
            .MaximumLength(255)
            .WithName(x => nameof(x.Data.Title));

        RuleFor(x => x.Data.ShortDescription)
            .NotEmpty()
            .MaximumLength(500)
            .WithName(x => nameof(x.Data.ShortDescription));

        RuleFor(x => x.Data.Address)
            .NotEmpty()
            .WithName(x => nameof(x.Data.Address))
            .WithMessage("The address is required");
    }
}