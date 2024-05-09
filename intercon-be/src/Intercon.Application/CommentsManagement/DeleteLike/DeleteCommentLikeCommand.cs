using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;

namespace Intercon.Application.CommentsManagement.DeleteLike;

public sealed record DeleteCommentLikeCommand(int CommentId, int CurrentUserId) : ICommand<int>;

internal sealed class DeleteCommentLikeCommandHandler
    (ICommentLikeRepository commentLikeRepository) : ICommandHandler<DeleteCommentLikeCommand, int>
{
    public async Task<int> Handle(DeleteCommentLikeCommand request, CancellationToken cancellationToken)
    {
        return await commentLikeRepository.Delete(request.CommentId, request.CurrentUserId);
    }
}