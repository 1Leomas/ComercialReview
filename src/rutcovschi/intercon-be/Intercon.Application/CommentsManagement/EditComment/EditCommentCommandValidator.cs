using FluentValidation;

namespace Intercon.Application.CommentsManagement.EditComment;

public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
{
    public EditCommentCommandValidator()
    {
        RuleFor(x => x.CommentDto.Id)
            .NotEmpty();

        RuleFor(x => x.CommentDto.Text)
            .NotEmpty()
            .MaximumLength(500)
            .WithName(x => nameof(x.CommentDto.Text));
    }
}