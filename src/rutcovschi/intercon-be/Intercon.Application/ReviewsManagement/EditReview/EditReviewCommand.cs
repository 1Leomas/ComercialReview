using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetReviewDetails;

namespace Intercon.Application.ReviewsManagement.EditReview;

public sealed record EditReviewDto(int AuthorId, int? Grade, string? ReviewText);

public sealed record EditReviewCommand(int BusinessId, EditReviewDto Data) : ICommand<ReviewDetailsDto>;

internal sealed class EditReviewCommandHandler(IReviewRepository reviewRepository) : ICommandHandler<EditReviewCommand, ReviewDetailsDto>
{
    public async Task<ReviewDetailsDto> Handle(EditReviewCommand command, CancellationToken cancellationToken)
    {
        var updatedReviewDb = await reviewRepository.UpdateReviewAsync(command.BusinessId, command.Data, cancellationToken);

        return updatedReviewDb!.ToDto();
    }
}