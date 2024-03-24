using FluentValidation;
using Intercon.Application.Abstractions;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed class CreateReviewCommandCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandCommandValidator(
        IBusinessRepository businessRepository,
        IUserRepository userRepository,
        IReviewRepository reviewRepository)
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

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithName(x => nameof(x.UserId))
            .DependentRules(() =>
            {
                RuleFor(x => x.UserId)
                    .MustAsync(userRepository.UserExistsAsync)
                    .WithMessage("The user doesn't exists");
            });

        RuleFor(x => new { x.BusinessId, x.UserId })
            .MustAsync(async (data, ctx)
                => !await reviewRepository.BusinessUserReviewExistsAsync(data.BusinessId, data.UserId, ctx))
            .WithName(x => nameof(x.UserId))
            .WithMessage("The user already wrote a review");

        RuleFor(x => x.Data.Grade)
            .NotEmpty()
            .InclusiveBetween(1, 5)
            .WithName(x => nameof(x.Data.Grade));

        RuleFor(x => x.Data.ReviewText)
            .MaximumLength(1000)
            .WithName(x => nameof(x.Data.ReviewText));
    }
}