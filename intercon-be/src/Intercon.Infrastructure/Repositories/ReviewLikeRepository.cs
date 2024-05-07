using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class ReviewLikeRepository(InterconDbContext dbContext) : IReviewLikeRepository
{
    private readonly InterconDbContext _dbContext = dbContext;

    public async Task<int> GetLikesCount(int businessId, int reviewAuthorId)
    {
        return await _dbContext.ReviewLikes.CountAsync(rl => 
            rl.BusinessId == businessId && 
            rl.ReviewAuthorId == reviewAuthorId);
    }

    public async Task<int> Add(int businessId, int reviewAuthorId, int userId)
    {
        var existingLike = await _dbContext.ReviewLikes.FirstOrDefaultAsync(rl =>
            rl.BusinessId == businessId &&
            rl.ReviewAuthorId == reviewAuthorId && 
            rl.UserId == userId);

        if (existingLike != null)
            return await GetLikesCount(businessId, reviewAuthorId);

        var reviewLike = new ReviewLike
        {
            BusinessId = businessId,
            ReviewAuthorId = reviewAuthorId,
            UserId = userId
        };

        _dbContext.ReviewLikes.Add(reviewLike);
        await _dbContext.SaveChangesAsync();

        return await GetLikesCount(businessId, reviewAuthorId);
    }

    public async Task<int> Delete(int businessId, int reviewAuthorId, int userId)
    {
        var likeToRemove = await _dbContext.ReviewLikes.FirstOrDefaultAsync(rl => 
            rl.BusinessId == businessId && 
            rl.ReviewAuthorId == reviewAuthorId && 
            rl.UserId == userId);

        if (likeToRemove == null)
            return await GetLikesCount(businessId, reviewAuthorId);

        _dbContext.ReviewLikes.Remove(likeToRemove);
        await _dbContext.SaveChangesAsync();

        return await GetLikesCount(businessId, reviewAuthorId);
    }
}