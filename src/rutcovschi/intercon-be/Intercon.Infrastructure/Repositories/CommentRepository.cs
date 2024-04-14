using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    public CommentRepository(InterconDbContext context)
    {
        _context = context;
    }

    private readonly InterconDbContext _context;

    public async Task<Comment?> GetCommentByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PaginatedList<Comment>> GetPaginatedCommentsByBusinessReviewAsync(
        int businessId, int reviewAuthorId, 
        CommentParameters parameters, 
        CancellationToken cancellationToken)
    {
        var commentsDb = _context.Comments
            .AsNoTracking()
            .Include(x => x.Author)
            .ThenInclude(x => x.Avatar)
            .Where(x => x.BusinessId == businessId && x.ReviewAuthorId == reviewAuthorId)
            .AsQueryable();

        commentsDb = ApplyFilter(commentsDb, parameters);

        commentsDb = ApplySort(commentsDb, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<Comment>.ToPagedList(
            commentsDb, 
            parameters.PageNumber,
            parameters.PageSize);
    }

    private IQueryable<Comment> ApplyFilter(IQueryable<Comment> comments, CommentParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.Search))
        {
            var search = parameters.Search;

            comments = comments.Where(
                x => x.Text.Contains(search));
        }

        return comments;
    }

    private IQueryable<Comment> ApplySort(
        IQueryable<Comment> comments,
        CommentSortBy sortBy,
        SortingDirection? direction = SortingDirection.Ascending)
    {
        return sortBy switch
        {
            CommentSortBy.UpdatedDate => comments.OrderUsing(x => x.UpdatedDate, direction ?? SortingDirection.Descending),
            CommentSortBy.CreatedDate => comments.OrderUsing(x => x.CreatedDate, direction ?? SortingDirection.Ascending),
            _ => comments.OrderUsing(x => x.UpdatedDate, SortingDirection.Descending)
        };
    }

    public async Task<IEnumerable<Comment>> GetCommentsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByBusinessIdAsync(int businessId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateCommentAsync(Comment newComment, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateCommentAsync(Comment newCommentData, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCommentAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CommentExistsAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}