using FluentValidation;
using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int AuthorId, int Grade, string? ReviewText);
public sealed record CreateReviewCommand(int BusinessId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler(IReviewRepository reviewRepository) : ICommandHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await reviewRepository.CreateReviewAsync(
            command.BusinessId,
            command.Data,
            cancellationToken);

        if (!result)
        {
            throw new InvalidOperationException("The review wasn't created");
        }
    }
}

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

        RuleFor(x => x.Data.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.Data.AuthorId))
            .DependentRules(() =>
            {
                RuleFor(x => x.Data.AuthorId)
                    .MustAsync(userRepository.UserExistsAsync)
                    .WithMessage("The user doesn't exists");
            });

        RuleFor(x => new {x.BusinessId, x.Data.AuthorId })
            .MustAsync(async (data, ctx) 
                => !(await reviewRepository.BusinessUserReviewExistsAsync(data.BusinessId, data.AuthorId, ctx)))
            .WithName(x => nameof(x.Data.AuthorId))
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
