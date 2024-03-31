using Intercon.Application.Abstractions;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Domain.Pagination;
using MediatR;

namespace Intercon.Application.ReviewsManagement.GetPaginatedBusinessReviews;

public sealed record GetPaginatedBusinessReviewsQuery(int BusinessId, ReviewParameters Parameters) : IRequest<PaginatedResponse<ReviewDetailsDto>>;

internal sealed class GetPaginatedBusinessReviewsQueryHandler
    (IReviewRepository reviewRepository) : IRequestHandler<GetPaginatedBusinessReviewsQuery, PaginatedResponse<ReviewDetailsDto>>
{
    public async Task<PaginatedResponse<ReviewDetailsDto>> Handle(GetPaginatedBusinessReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews =
            await reviewRepository.GetPaginatedBusinessReviewsAsync(request.BusinessId, request.Parameters, cancellationToken);

        return new PaginatedResponse<ReviewDetailsDto>()
        {
            CurrentPage = reviews.CurrentPage,
            TotalPages = reviews.TotalPages,
            PageSize = reviews.PageSize,
            TotalCount = reviews.TotalCount,
            HasPrevious = reviews.HasPrevious,
            HasNext = reviews.HasNext,
            Items = reviews.Select(x => x.ToDetailedDto()).ToList()
        };
    }
}