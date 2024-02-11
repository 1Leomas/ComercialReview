using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.ReviewsManagement.GetReview;

public record ReviewDto(
    int Id,
    int BusinessId,
    int AuthorId,
    UserDto Author,
    float Grade,
    string ReviewText
);

public record GetReviewQuery(int Id) : IQuery<ReviewDto?>;

public class GetReviewQueryHandler(InterconDbContext context) : IQueryHandler<GetReviewQuery, ReviewDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<ReviewDto?> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var reviewFromDb = await _context.Reviews
            .Include(review => review.Author)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return reviewFromDb?.ToDto();
    }
}