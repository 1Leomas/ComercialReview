using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class CommentLikeRepository(InterconDbContext dbContext) : ICommentLikeRepository
{
    private readonly InterconDbContext _dbContext = dbContext;

    public async Task<int> GetLikesCount(int commentId, int userId)
    {
        return await _dbContext.CommentLikes.CountAsync(cl =>
            cl.CommentId == commentId &&
            cl.UserId == userId);
    }

    public async Task<int> Add(int commentId, int userId)
    {
        var existingLike = await _dbContext.CommentLikes.FirstOrDefaultAsync(cl =>
            cl.CommentId == commentId &&
            cl.UserId == userId);

        if (existingLike != null)
            return await GetLikesCount(commentId, userId);

        var commentLike = new CommentLike
        {
            CommentId = commentId,
            UserId = userId
        };

        _dbContext.CommentLikes.Add(commentLike);
        await _dbContext.SaveChangesAsync();

        return await GetLikesCount(commentId, userId);
    }

    public async Task<int> Delete(int commentId, int userId)
    {
        var likeToRemove = await _dbContext.CommentLikes.FirstOrDefaultAsync(cl =>
            cl.CommentId == commentId &&
            cl.UserId == userId);

        if (likeToRemove == null)
            return await GetLikesCount(commentId, userId);

        _dbContext.CommentLikes.Remove(likeToRemove);
        await _dbContext.SaveChangesAsync();

        return await GetLikesCount(commentId, userId);
    }
}