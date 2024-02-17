﻿using Intercon.Application.Extensions.Mappers;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.GetBusinessReviews;

public sealed record GetBusinessReviewsQuery(int BusinessId) : IRequest<IEnumerable<ReviewDetailsDto>>;

internal sealed class GetBusinessReviewsHandler(InterconDbContext context) : IRequestHandler<GetBusinessReviewsQuery, IEnumerable<ReviewDetailsDto>>
{
    readonly InterconDbContext _context = context;

    public async Task<IEnumerable<ReviewDetailsDto>> Handle(GetBusinessReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .Include(review => review.Author)
            .Where(x => x.BusinessId == request.BusinessId)
            .ToListAsync(cancellationToken);

        return reviews.Select(x => x.ToDto());
    }
}
