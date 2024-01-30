using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Reviews.GetReview;

public record GetReviewQuery(int Id) : IQuery<ReviewDto?>;

public class GetReviewQueryHandler(InterconDbContext context) : IQueryHandler<GetReviewQuery, ReviewDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<ReviewDto?> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var reviewFromDb = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == request.Id);

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
            Rating = reviewFromDb.Rating,
            ReviewTitle = reviewFromDb.ReviewTitle,
            ReviewText = reviewFromDb.ReviewText,
            CreateDate = reviewFromDb.CreateDate,
            UpdateDate = reviewFromDb.UpdateDate,
            WasEdited = reviewFromDb.WasEdited
        };
    }
}

public class ReviewDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int AuthorId { get; set; }

    public virtual UserDto Author { get; set; }
    public float Rating { get; set; }
    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; }
    public bool WasEdited { get; set; }
}
