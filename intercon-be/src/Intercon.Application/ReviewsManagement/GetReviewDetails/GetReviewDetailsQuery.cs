using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.ReviewsManagement.GetReviewDetails;

public record ReviewDetailsDto(
    int BusinessId,
    int AuthorId,
    ReviewAuthorDto Author,
    float Grade,
    string? ReviewText,
    int RecommendationType,
    uint CommentsCount,
    int LikesCount,
    bool CurrentUserLiked,
    DateTime CreatedDate,
    DateTime UpdatedDate,
    bool WasEdited
);

public sealed record GetReviewDetailsQuery(int BusinessId, int AuthorId, int? CurrentUserId) : IQuery<ReviewDetailsDto?>;

internal sealed class GetReviewDetailsHandler
    (IReviewRepository reviewRepository) : IQueryHandler<GetReviewDetailsQuery, ReviewDetailsDto?>
{
    public async Task<ReviewDetailsDto?> Handle(GetReviewDetailsQuery request, CancellationToken cancellationToken)
    {
        var reviewFromDb =
            await reviewRepository.GetReviewDetailsAsync(request.BusinessId, request.AuthorId, cancellationToken);

        return reviewFromDb?.ToDetailedDto(request.CurrentUserId);
    }
}