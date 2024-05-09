using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int Grade, string? ReviewText, int RecommendationType);

public sealed record CreateReviewCommand(int BusinessId, int UserId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler(IReviewRepository reviewRepository)
    : ICommandHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var review = Review.Create(
            command.BusinessId,
            command.UserId,
            command.Data.Grade,
            command.Data.ReviewText,
            (RecommendationType)command.Data.RecommendationType);

        var result = await reviewRepository.CreateReviewAsync(review, cancellationToken);

        if (!result) throw new InvalidOperationException("The review wasn't created");
    }
}