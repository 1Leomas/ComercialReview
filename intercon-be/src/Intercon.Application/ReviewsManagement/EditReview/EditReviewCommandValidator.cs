using FluentValidation;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Enums;

namespace Intercon.Application.ReviewsManagement.EditReview;

public sealed class EditReviewCommandValidator : AbstractValidator<EditReviewCommand>
{
    public EditReviewCommandValidator(IUserRepository userRepository, IBusinessRepository businessRepository)
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty()
            .WithName(x => nameof(x.BusinessId))
            .DependentRules(() =>
            {
                RuleFor(x => x.BusinessId)
                    .MustAsync(businessRepository.BusinessExistsAsync)
                    .WithMessage("The business doesn't exists");
            });

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.AuthorId))
            .DependentRules(() =>
            {
                RuleFor(x => x.AuthorId)
                    .MustAsync(userRepository.UserExistsAsync)
                    .WithMessage("The user doesn't exists");
            });

        When(x => x.Data.Grade is not null, () =>
        {
            RuleFor(x => x.Data.Grade)
                .InclusiveBetween(1, 5)
                .WithName(x => nameof(x.Data.Grade));
        });

        When(x => x.Data.ReviewText is not null, () =>
        {
            RuleFor(x => x.Data.ReviewText)
                .MaximumLength(1000)
                .WithName(x => nameof(x.Data.ReviewText));
        });

        When(x => x.Data.RecommendationType is not null, () =>
        {
            RuleFor(x => x.Data.RecommendationType)
                .Must(x => Enum.IsDefined(typeof(RecommendationType), x))
                .WithName(x => nameof(x.Data.RecommendationType))
                .WithMessage("The recommendation type is invalid");
        });
    }
}