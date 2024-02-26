using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.GetReviewDetails;

public record ReviewDetailsDto(
    int BusinessId,
    int AuthorId,
    ReviewAuthorDto Author,
    float Grade,
    string? ReviewText,
    DateTime CreateDate,
    DateTime UpdateDate,
    bool WasEdited
);

public sealed record GetReviewDetailsQuery(int BusinessId, int AuthorId) : IQuery<ReviewDetailsDto?>;

internal sealed class GetReviewDetailsHandler(InterconDbContext context) : IQueryHandler<GetReviewDetailsQuery, ReviewDetailsDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<ReviewDetailsDto?> Handle(GetReviewDetailsQuery request, CancellationToken cancellationToken)
    {
        var reviewFromDb = await _context.Reviews
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .FirstOrDefaultAsync(x => x.BusinessId == request.BusinessId && x.AuthorId == request.AuthorId, cancellationToken);

        return reviewFromDb?.ToDto();
    }
}