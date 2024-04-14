using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Enums;

namespace Intercon.Application.ReviewsManagement.EditReview;

public sealed record UpdatedReviewDto(int BusinessId, int AuthorId, float Grade, string? ReviewText, int Like);

public sealed record EditReviewDto(int? Grade, string? ReviewText, int? Like);

public sealed record EditReviewCommand(int BusinessId, int AuthorId, EditReviewDto Data) : ICommand<UpdatedReviewDto>;

internal sealed class EditReviewCommandHandler
    (IReviewRepository reviewRepository) : ICommandHandler<EditReviewCommand, UpdatedReviewDto>
{
    public async Task<UpdatedReviewDto> Handle(EditReviewCommand command, CancellationToken cancellationToken)
    {
        var updatedReviewDb =
            await reviewRepository.UpdateReviewAsync(command.BusinessId, command.AuthorId, command.Data, cancellationToken);

        if (updatedReviewDb is null) throw new InvalidOperationException("Can not update review");

        return new UpdatedReviewDto(
            updatedReviewDb.BusinessId,
            updatedReviewDb.AuthorId,
            updatedReviewDb.Grade,
            updatedReviewDb.ReviewText,
            (int)updatedReviewDb.Recommendation
        );
    }
}