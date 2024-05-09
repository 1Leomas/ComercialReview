using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Application.Exceptions;
using Intercon.Domain.Entities;

namespace Intercon.Application.CommentsManagement.EditComment;

public sealed record EditCommentCommand(EditCommentDto CommentDto) : ICommand;

internal sealed class EditCommentCommandHandler : ICommandHandler<EditCommentCommand>
{
    public EditCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    private readonly ICommentRepository _commentRepository;

    public async Task Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentDto.Id, cancellationToken);

        if (comment is null || comment.AuthorId != request.CommentDto.AuthorId)
        {
            throw new NotFoundException(nameof(Comment));
        }

        comment.Text = request.CommentDto.Text;
        comment.UpdatedDate = DateTime.Now;

        var result = await _commentRepository.EditAsync(comment, cancellationToken);

        if (!result) throw new InternalException();
    }
}