using FluentValidation;
using Intercon.Application.Abstractions.Repositories;

namespace Intercon.Application.CommentsManagement.AddComment;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator(IReviewRepository reviewRepository)
    {
        RuleFor(x => x.CommentDto.Text).NotEmpty().MaximumLength(500);

        RuleFor(x => new { x.CommentDto.BusinessId, x.CommentDto.ReviewAuthorId })
            .MustAsync(async (data, ctx)
                => await reviewRepository.ReviewExistsAsync(data.BusinessId, data.ReviewAuthorId, ctx))
            .WithMessage("The review doesn't exists")
            .WithName("Review");
    }
}