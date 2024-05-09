namespace Intercon.Application.Abstractions.Repositories;

public interface ICommentLikeRepository
{
    Task<int> GetLikesCount(int commentId, int userId);
    Task<int> Add(int commentId, int userId);
    Task<int> Delete(int commentId, int userId);
}