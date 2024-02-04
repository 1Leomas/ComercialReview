using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.GetReview;

public record GetReviewQuery(int Id) : IQuery<ReviewDto?>;

public class GetReviewQueryHandler(InterconDbContext context) : IQueryHandler<GetReviewQuery, ReviewDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<ReviewDto?> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var reviewFromDb = await _context.Reviews
            .Include(review => review.Author)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (reviewFromDb == null)
        {
            return null;
        }

        return new ReviewDto()
        {
            Id = reviewFromDb.Id,
            BusinessId = reviewFromDb.BusinessId,
            AuthorId = reviewFromDb.AuthorId,
            Author = new UserDto()
            {
                Id = reviewFromDb.AuthorId,
                FirstName = reviewFromDb.Author.FirstName,
                LastName = reviewFromDb.Author.LastName,
                Email = reviewFromDb.Author.Email
            },
            Grade = reviewFromDb.Grade,
            ReviewText = reviewFromDb.ReviewText,
        };
    }
}

public class ReviewDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int AuthorId { get; set; }
    public virtual UserDto Author { get; set; } = null!;
    public float Grade { get; set; }
    public string ReviewText { get; set; } = string.Empty;
}
