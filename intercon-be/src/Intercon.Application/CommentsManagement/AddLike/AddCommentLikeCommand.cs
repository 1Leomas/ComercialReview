using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.CommentsManagement.AddLike;

public sealed record AddCommentLikeCommand(int CommentId, int CurrentUserId) : ICommand<int>;

internal sealed class AddCommentLikeCommandHandler
    (ICommentLikeRepository commentLikeRepository) : ICommandHandler<AddCommentLikeCommand, int>
{
    public async Task<int> Handle(AddCommentLikeCommand request, CancellationToken cancellationToken)
    {
        return await commentLikeRepository.Add(request.CommentId, request.CurrentUserId);
    }
}