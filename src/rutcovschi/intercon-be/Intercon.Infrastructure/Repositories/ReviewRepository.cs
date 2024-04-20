using Intercon.Application.Abstractions;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PaginatedList<Review>> GetPaginatedBusinessReviewsAsync(int businessId, ReviewParameters parameters, CancellationToken cancellationToken)
    {
        var reviews = context.Reviews
            .AsNoTracking()
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .Where(x => x.BusinessId == businessId)
            .AsQueryable();

        reviews = ApplyFilter(reviews, parameters);

        reviews = ApplySort(reviews, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<Review>.ToPagedList(
            reviews, 
            parameters.PageNumber,
            parameters.PageSize);
    }

    private IQueryable<Review> ApplySort(
        IQueryable<Review> review,
        ReviewSortBy sortBy,
        SortingDirection? direction = SortingDirection.Ascending)
    {
        return sortBy switch
        {
            ReviewSortBy.UpdatedDate => review.OrderUsing(x => x.UpdatedDate, direction ?? SortingDirection.Descending),
            ReviewSortBy.Grade => review.OrderUsing(x => x.Grade, direction ?? SortingDirection.Ascending),
            ReviewSortBy.Like => review.OrderUsing(x => x.Recommendation, direction ?? SortingDirection.Ascending),
            _ => review.OrderUsing(x => x.UpdatedDate, SortingDirection.Descending)
        };
    }

    private IQueryable<Review> ApplyFilter(IQueryable<Review> reviews, ReviewParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.Search))
        {
            var search = parameters.Search.ToLower();

            reviews = reviews.Where(x =>
                x.ReviewText != null && x.ReviewText.ToLower().Contains(search) ||
                x.Author.FirstName.ToLower().Contains(search) ||
                x.Author.LastName.ToLower().Contains(search));
        }

        if (parameters.Grades.Any() && !parameters.Grades.Contains(ReviewGrade.All))
        {
            reviews = reviews.Where(x => parameters.Grades.Contains((ReviewGrade)x.Grade));
        }

        if (parameters.RecommendationType != RecommendationType.Neutral)
        {
            reviews = reviews.Where(x => x.Recommendation == parameters.RecommendationType);
        }

        return reviews;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync(CancellationToken cancellationToken)
    {
        return await context.Reviews.ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateReviewAsync(int businessId, int userId, CreateReviewDto newReview, CancellationToken cancellationToken)
    {
        var date = DateTime.Now;

        var review = new Review
        {
            BusinessId = businessId,
            AuthorId = userId,
            Grade = newReview.Grade,
            ReviewText = newReview.ReviewText,
            Recommendation = (RecommendationType)newReview.RecommendationType,
            CreatedDate = date,
            UpdatedDate = date
        };

        await context.Reviews.AddAsync(review, cancellationToken);
        var rows = await context.SaveChangesAsync(cancellationToken);

        if (rows == 0)
        {
            return false;
        }

        await UpdateBusinessStats(businessId, cancellationToken);

        return true;
    }

    public async Task<Review?> UpdateReviewAsync(
        int businessId, 
        int authorId, 
        EditReviewDto newReviewData, 
        CancellationToken cancellationToken)
    {
        var reviewDb = await context.Reviews.FindAsync(businessId, authorId, cancellationToken);

        if (reviewDb == null)
        {
            return null;
        }

        if (newReviewData.Grade is >= 1 and <= 5)
        {
            reviewDb.Grade = newReviewData.Grade.Value;
        }
        if (newReviewData.ReviewText != null)
        {
            reviewDb.ReviewText = newReviewData.ReviewText;
        }
        if (newReviewData.Like != null)
        {
            reviewDb.Recommendation = (RecommendationType)newReviewData.Like.Value;
        }

        reviewDb.UpdatedDate = DateTime.Now;

        await context.SaveChangesAsync(cancellationToken);

        await UpdateBusinessStats(businessId, cancellationToken);

        return reviewDb;
    }

    public async Task DeleteReviewAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        var reviewDb = await context.Reviews.FindAsync(businessId, authorId);

        if (reviewDb == null)
        {
            return;
        }
        
        await context.Reviews
            .Where(x => x.BusinessId == businessId && x.AuthorId == authorId)
            .ExecuteDeleteAsync(cancellationToken);

        await UpdateBusinessStats(businessId, cancellationToken);
    }

    private async Task UpdateBusinessStats(
        int businessId,
        CancellationToken cancellationToken)
    {
        var business = await context.Businesses.FindAsync(businessId, cancellationToken);

        if (business == null)
        {
            return;
        }

        var reviews = context.Reviews.Where(x => x.BusinessId == businessId);

        var reviewsCount = (uint)(reviews.Count());
        float reviewsSum = reviews.Select(x => x.Grade).Sum();

        business!.ReviewsCount = reviewsCount;
        business.Rating = reviewsSum / reviewsCount;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ReviewExistsAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        return await context.Reviews.AnyAsync(
            x => x.BusinessId == businessId && x.AuthorId == authorId, 
            cancellationToken);
    }
}