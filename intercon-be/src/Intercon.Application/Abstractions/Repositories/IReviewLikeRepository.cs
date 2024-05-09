namespace Intercon.Application.Abstractions.Repositories;

public interface IReviewLikeRepository
{
    Task<int> GetLikesCount(int businessId, int reviewAuthorId);
    Task<int> Add(int businessId, int reviewAuthorId, int userId);
    Task<int> Delete(int businessId, int reviewAuthorId, int userId);
}