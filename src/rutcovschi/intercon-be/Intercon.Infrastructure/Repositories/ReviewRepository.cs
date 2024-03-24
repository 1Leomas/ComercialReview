﻿using Intercon.Application.Abstractions;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Domain.Entities;
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

    public async Task<IEnumerable<Review>> GetAllReviewsAsync(CancellationToken cancellationToken)
    {
        return await context.Reviews.ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateReviewAsync(int businessId, int userId, CreateReviewDto newReview, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            BusinessId = businessId,
            AuthorId = userId,
            Grade = newReview.Grade,
            ReviewText = newReview.ReviewText
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
        var reviewDb = await context.Reviews.FindAsync(businessId, authorId);

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

        reviewDb.UpdateDate = DateTime.Now;
        reviewDb.WasEdited = true;

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

    public async Task<bool> BusinessUserReviewExistsAsync(int businessId, int authorId, CancellationToken cancellationToken)
    {
        return await context.Reviews.AnyAsync(
            x => x.BusinessId == businessId && x.AuthorId == authorId, 
            cancellationToken);
    }
}