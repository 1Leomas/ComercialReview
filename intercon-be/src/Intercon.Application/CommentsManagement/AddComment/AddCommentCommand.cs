using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Application.Exceptions;
using Intercon.Domain.Entities;

namespace Intercon.Application.CommentsManagement.AddComment;

public sealed record AddCommentCommand(AddCommentDto CommentDto) : ICommand;

internal sealed class AddCommentCommandHandler : ICommandHandler<AddCommentCommand>
{
    public AddCommentCommandHandler(ICommentRepository commentRepository, IBusinessRepository businessRepository)
    {
        _commentRepository = commentRepository;
        _businessRepository = businessRepository;
    }

    private readonly ICommentRepository _commentRepository;
    private readonly IBusinessRepository _businessRepository;

    public async Task Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var dateNow = DateTime.Now;

        var comment = new Comment
        {
            Text = request.CommentDto.Text,
            IsCommentOfBusinessOwner = await _businessRepository.UserOwnsBusinessAsync(request.CommentDto.AuthorId, request.CommentDto.BusinessId, cancellationToken),
            BusinessId = request.CommentDto.BusinessId,
            ReviewAuthorId = request.CommentDto.ReviewAuthorId,
            AuthorId = request.CommentDto.AuthorId,
            CreatedDate = dateNow,
            UpdatedDate = dateNow
        };

        var result = await _commentRepository.AddAsync(comment, cancellationToken);

        if (!result) throw new InternalException();
    }
}