using Intercon.Application.Abstractions;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Intercon.Infrastructure.Repositories;

public class ReviewRepository(InterconDbContext context) 
    : IReviewRepository
{
    public async Task<Review?> GetReviewDetailsAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.AuthorId == authorId, cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetBusinessReviewsAsync(int businessId, CancellationToken cancellationToken)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .Where(x => x.BusinessId == businessId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync(CancellationToken cancellationToken)
    {
        return await context.Reviews.ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateReviewAsync(int businessId, CreateReviewDto newReview, CancellationToken cancellationToken)
    {
        var business = await context.Businesses.FindAsync(businessId, cancellationToken);

        if (business == null)
        {
            return false;
        }

        var review = new Review
        {
            BusinessId = businessId,
            AuthorId = newReview.AuthorId,
            Grade = newReview.Grade,
            ReviewText = newReview.ReviewText
        };

        var reviews = context.Reviews.Where(x => x.BusinessId == businessId);

        var reviewsCount = (uint)(reviews.Count() + 1);
        float reviewsSum = reviews.Select(x => x.Grade).Sum() + newReview.Grade;

        business!.ReviewsCount = reviewsCount;
        business.Rating = reviewsSum / reviewsCount;

        await context.Reviews.AddAsync(review, cancellationToken);
        var rows = await context.SaveChangesAsync(cancellationToken);

        return rows != 0;
    }

    public async Task<Review?> UpdateReviewAsync(int businessId, EditReviewDto newReviewData, CancellationToken cancellationToken)
    {
        var review = await context.Reviews.FindAsync(businessId, newReviewData.AuthorId);

        if (review == null)
        {
            return null;
        }

        if (newReviewData.Grade is > 0 and <= 5)
        {
            review.Grade = newReviewData.Grade.Value;
        }
        if (newReviewData.ReviewText != null)
        {
            review.ReviewText = newReviewData.ReviewText;
        }

        await context.SaveChangesAsync(cancellationToken);

        return review;
    }

    public async Task DeleteReviewAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        await context.Reviews
            .Where(x => x.BusinessId == businessId && x.AuthorId == authorId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> BusinessUserReviewExistsAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        return await context.Reviews.AnyAsync(
            x => x.BusinessId == businessId && x.AuthorId == authorId, 
            cancellationToken);
    }
}