using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IReviewRepository
{
    Task<Review?> GetReviewDetailsAsync(int businessId, int authorId, CancellationToken cancellationToken);
    Task<IEnumerable<Review>> GetBusinessReviewsAsync(int businessId, CancellationToken cancellationToken);
    Task<IEnumerable<Review>> GetAllReviewsAsync(CancellationToken cancellationToken);
    Task<bool> CreateReviewAsync(int businessId, int userId, CreateReviewDto newReview, CancellationToken cancellationToken);
    Task<Review?> UpdateReviewAsync(int businessId, int authorId, EditReviewDto newReviewData, CancellationToken cancellationToken);
    Task DeleteReviewAsync(int businessId, int authorId, CancellationToken cancellationToken);
    Task<bool> BusinessUserReviewExistsAsync(int businessId, int authorId, CancellationToken cancellationToken);
}