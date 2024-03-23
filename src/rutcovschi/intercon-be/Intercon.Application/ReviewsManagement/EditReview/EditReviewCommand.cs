using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetReviewDetails;

namespace Intercon.Application.ReviewsManagement.EditReview;

public sealed record UpdatedReviewDto(int BusinessId, int AuthorId, float Grade, string? ReviewText);

public sealed record EditReviewDto(int AuthorId, int? Grade, string? ReviewText);

public sealed record EditReviewCommand(int BusinessId, EditReviewDto Data) : ICommand<UpdatedReviewDto>;

internal sealed class EditReviewCommandHandler(IReviewRepository reviewRepository) : ICommandHandler<EditReviewCommand, UpdatedReviewDto>
{
    public async Task<UpdatedReviewDto> Handle(EditReviewCommand command, CancellationToken cancellationToken)
    {
        var updatedReviewDb = await reviewRepository.UpdateReviewAsync(command.BusinessId, command.Data, cancellationToken);

        if (updatedReviewDb is null)
        {
            throw new InvalidOperationException("Can not update review");
        }

        return new UpdatedReviewDto(
            BusinessId: updatedReviewDb.BusinessId,
            AuthorId: updatedReviewDb.AuthorId,
            Grade: updatedReviewDb.Grade,
            ReviewText: updatedReviewDb.ReviewText
        );
    }
}