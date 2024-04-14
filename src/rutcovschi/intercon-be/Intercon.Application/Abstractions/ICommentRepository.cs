using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;

namespace Intercon.Application.Abstractions;

public interface ICommentRepository
{
    Task<Comment?> GetCommentByIdAsync(int id, CancellationToken cancellationToken);
    Task<PaginatedList<Comment>> GetPaginatedCommentsByBusinessReviewAsync(int businessId, int reviewAuthorId, CommentParameters parameters, CancellationToken cancellationToken);
    Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<Comment>> GetCommentsByBusinessIdAsync(int businessId, CancellationToken cancellationToken);
    Task<bool> AddAsync(Comment comment, CancellationToken cancellationToken);
    Task<bool> UpdateCommentAsync(Comment newCommentData, CancellationToken cancellationToken);
    Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken);
    Task<bool> CommentExistsAsync(int id, CancellationToken cancellationToken);
}