using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CA1862

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
            .Include(r => r.Likes)
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.AuthorId == authorId, cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetBusinessReviewsAsync(int businessId, CancellationToken cancellationToken)
    {
        return await context.Reviews
            .AsNoTracking()
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .Include(r => r.Likes)
            .Where(x => x.BusinessId == businessId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<Review>> GetPaginatedBusinessReviewsAsync(int businessId, ReviewParameters parameters, CancellationToken cancellationToken)
    {
        var reviews = context.Reviews
            .AsNoTracking()
            .Include(review => review.Author)
            .ThenInclude(user => user.Avatar)
            .Include(r => r.Likes)
            .Where(x => x.BusinessId == businessId)
            .AsQueryable();

        reviews = ApplyFilter(reviews, parameters);

        reviews = ApplySort(reviews, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<Review>.ToPagedList(
            reviews,
            parameters.PageNumber,
            parameters.PageSize);
    }

    public async Task<IEnumerable<Review>> GetAllReviewsAsync(CancellationToken cancellationToken)
    {
        return await context.Reviews.ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateReviewAsync(Review review, CancellationToken cancellationToken)
    {
        await context.Reviews.AddAsync(review, cancellationToken);
        var rows = await context.SaveChangesAsync(cancellationToken);

        if (rows == 0) return false;

        await UpdateBusinessStats(review.BusinessId, cancellationToken);

        return true;
    }

    public async Task<Review?> UpdateReviewAsync(
        int businessId,
        int authorId,
        EditReviewDto newReviewData,
        CancellationToken cancellationToken)
    {
        var reviewDb = await context.Reviews.FindAsync(
            new object?[] { businessId, authorId },
            cancellationToken: cancellationToken);

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
        if (newReviewData.RecommendationType != null)
        {
            reviewDb.Recommendation = (RecommendationType)newReviewData.RecommendationType.Value;
        }

        reviewDb.UpdatedDate = DateTime.Now;

        await context.SaveChangesAsync(cancellationToken);

        await UpdateBusinessStats(businessId, cancellationToken);

        return reviewDb;
    }

    public async Task DeleteReviewAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        var reviewDb = await context.Reviews.FindAsync(
            new object?[] { businessId, authorId },
            cancellationToken: cancellationToken);

        if (reviewDb == null)
        {
            return;
        }

        context.Reviews.Remove(reviewDb);

        await UpdateBusinessStats(businessId, cancellationToken);
    }

    public async Task<bool> ReviewExistsAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        return await context.Reviews.AnyAsync(
            x => x.BusinessId == businessId && x.AuthorId == authorId,
            cancellationToken);
    }

    private static IQueryable<Review> ApplySort(
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

    private static IQueryable<Review> ApplyFilter(IQueryable<Review> reviews, ReviewParameters parameters)
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

    private async Task UpdateBusinessStats(
        int businessId,
        CancellationToken cancellationToken)
    {
        var business = await context.Businesses.FindAsync(
            new object?[] { businessId },
            cancellationToken: cancellationToken);

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
}