using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Domain.Enums;

namespace Intercon.Application.CommentsManagement.DeleteComment;

public sealed record DeleteCommentCommand(DeleteCommentRequest DeleteCommentRequest) : ICommand;

internal sealed class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
{
    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    private readonly ICommentRepository _commentRepository;

    public async Task Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(command.DeleteCommentRequest.Id, cancellationToken);

        if (comment == null) return;

        var userId = command.DeleteCommentRequest.CurrentUserId;
        var userRole = command.DeleteCommentRequest.CurrentUserRole;

        if (comment.AuthorId != userId
            && userRole != Role.SuperAdmin)
        {
            throw new InvalidOperationException("You are not the author of this comment");
        }

        await _commentRepository.DeleteAsync(comment.Id, cancellationToken);
    }
}
