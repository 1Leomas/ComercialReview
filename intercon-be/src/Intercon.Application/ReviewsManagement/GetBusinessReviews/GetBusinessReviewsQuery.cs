using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using MediatR;

namespace Intercon.Application.ReviewsManagement.GetBusinessReviews;

public sealed record GetBusinessReviewsQuery(int BusinessId) : IRequest<IEnumerable<ReviewDetailsDto>>;

internal sealed class GetBusinessReviewsHandler
    (IReviewRepository reviewRepository) : IRequestHandler<GetBusinessReviewsQuery, IEnumerable<ReviewDetailsDto>>
{
    public async Task<IEnumerable<ReviewDetailsDto>> Handle(GetBusinessReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await reviewRepository.GetBusinessReviewsAsync(request.BusinessId, cancellationToken);

        return reviews.Select(x => x.ToDetailedDto());
    }
}