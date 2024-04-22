using Intercon.Application.Abstractions;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Enums;
using MediatR;

namespace Intercon.Application.ReviewsManagement.GetAllReviews;

public record ReviewShortDto(
    int BusinessId,
    int AuthorId,
    float Grade,
    string? ReviewText,
    int Like);

public sealed record GetAllReviewsQuery : IRequest<IEnumerable<ReviewShortDto>>;

internal sealed class GetAllReviewsQueryHandler
    (IReviewRepository reviewRepository) : IRequestHandler<GetAllReviewsQuery, IEnumerable<ReviewShortDto>>
{
    public async Task<IEnumerable<ReviewShortDto>> Handle(GetAllReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetAllReviewsAsync(cancellationToken);

        return reviews.Select(x => x.ToShortDto());
    }
}