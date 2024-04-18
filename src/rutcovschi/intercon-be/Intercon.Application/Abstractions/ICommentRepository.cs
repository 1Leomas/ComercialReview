using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;

namespace Intercon.Application.Abstractions;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PaginatedList<Comment>> GetPaginatedCommentsByBusinessReviewAsync(int businessId, int reviewAuthorId, CommentParameters parameters, CancellationToken cancellationToken);
    Task<IEnumerable<Comment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<Comment>> GetByBusinessIdAsync(int businessId, CancellationToken cancellationToken);
    Task<bool> AddAsync(Comment comment, CancellationToken cancellationToken);
    Task<bool> EditAsync(Comment newCommentData, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> CommentExistsAsync(int id, CancellationToken cancellationToken);
}