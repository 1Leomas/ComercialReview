using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetBusinessReviews;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.GetAllReviews;

public record ReviewShortDto(
       int BusinessId,
        int AuthorId,
        float Grade,
        string? ReviewText);

public sealed record GetAllReviewsQuery() : IRequest<IEnumerable<ReviewShortDto>>;

internal sealed class GetAllReviewsQueryHandler(InterconDbContext context) : IRequestHandler<GetAllReviewsQuery, IEnumerable<ReviewShortDto>>
{
    readonly InterconDbContext _context = context;

    public async Task<IEnumerable<ReviewShortDto>> Handle(GetAllReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .ToListAsync(cancellationToken);

        return reviews.Select(x => x.ToShortDto());
    }
}