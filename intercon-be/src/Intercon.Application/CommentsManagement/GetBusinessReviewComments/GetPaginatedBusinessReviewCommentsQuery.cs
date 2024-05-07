using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Pagination;

namespace Intercon.Application.CommentsManagement.GetBusinessReviewComments;

public sealed record GetPaginatedBusinessReviewCommentsQuery(int BusinessId, int ReviewAuthorId, int? CurrentUserId, CommentParameters Parameters) : IQuery<PaginatedResponse<CommentDetailsDto>>;

internal sealed class GetPaginatedBusinessReviewCommentsQueryHandler 
    : IQueryHandler<GetPaginatedBusinessReviewCommentsQuery, PaginatedResponse<CommentDetailsDto>>
{
    public GetPaginatedBusinessReviewCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    private readonly ICommentRepository _commentRepository;

    public async Task<PaginatedResponse<CommentDetailsDto>> Handle(GetPaginatedBusinessReviewCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.GetPaginatedCommentsByBusinessReviewAsync(
            request.BusinessId, 
            request.ReviewAuthorId, 
            request.Parameters,
        cancellationToken);

        return new PaginatedResponse<CommentDetailsDto>()
        {
            CurrentPage = comments.CurrentPage,
            TotalPages = comments.TotalPages,
            PageSize = comments.PageSize,
            TotalCount = comments.TotalCount,
            HasPrevious = comments.HasPrevious,
            HasNext = comments.HasNext,
            Items = comments.Select(x => x.ToDetailsDto(request.CurrentUserId)).ToList()
        };
    }
}
