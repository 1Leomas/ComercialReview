using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int AuthorId, int Grade, string? ReviewText);

public sealed record CreateReviewCommand(int BusinessId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler
    (IReviewRepository reviewRepository) : ICommandHandler<CreateReviewCommand>
{
    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var result = await reviewRepository.CreateReviewAsync(
            command.BusinessId,
            command.Data,
            cancellationToken);

        if (!result) throw new InvalidOperationException("The review wasn't created");
    }
}