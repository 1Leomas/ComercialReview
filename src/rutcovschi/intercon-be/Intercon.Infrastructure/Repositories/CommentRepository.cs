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

    public async Task<Comment?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Comments            
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
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
            var search = parameters.Search.ToLower();

            comments = comments.Where(
                x => x.Text.ToLower().Contains(search));
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

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Comment>> GetByBusinessIdAsync(int businessId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        _context.Comments.Add(comment);
        var rows = await _context.SaveChangesAsync(cancellationToken);

        if (rows > 0)
        {
            await UpdateReviewStats(comment.BusinessId, comment.ReviewAuthorId, cancellationToken);
        }

        return rows > 0;
    }

    public async Task<bool> EditAsync(Comment newCommentData, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments.FindAsync(newCommentData.Id, cancellationToken);
        
        if (comment == null) return false;

        comment.Text = newCommentData.Text;
        comment.UpdatedDate = DateTime.Now;

        var rows = await _context.SaveChangesAsync(cancellationToken);

        return rows > 0;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (comment == null)
        {
            return;
        }
        
        _context.Comments.Remove(comment);

        await UpdateReviewStats(comment.BusinessId, comment.ReviewAuthorId, cancellationToken);
    }

    public async Task<bool> CommentExistsAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateReviewStats(
        int businessId,
        int reviewAuthorId,
        CancellationToken cancellationToken)
    {
        var review = await _context.Reviews.FindAsync(businessId, reviewAuthorId, cancellationToken);

        if (review == null)
        {
            return;
        }

        var comments = _context.Comments
            .Where(x => x.BusinessId == businessId && x.ReviewAuthorId == reviewAuthorId);

        var commentsCount = (uint)(comments.Count());

        review!.CommentsCount = commentsCount;

        await _context.SaveChangesAsync(cancellationToken);
    }
}