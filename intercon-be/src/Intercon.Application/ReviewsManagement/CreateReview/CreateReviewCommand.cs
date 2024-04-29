using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Enums;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int Grade, string? ReviewText, int RecommendationType);

public sealed record CreateReviewCommand(int BusinessId, int UserId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler
    (IReviewRepository reviewRepository) : ICommandHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await reviewRepository.CreateReviewAsync(
            command.BusinessId,
            command.UserId,
            command.Data,
            cancellationToken);

        if (!result) throw new InvalidOperationException("The review wasn't created");
    }
}