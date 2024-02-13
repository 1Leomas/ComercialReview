using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Application.ReviewsManagement.EditReview;

public sealed record EditReviewDto(int AuthorId, int Grade, string? ReviewText);

public sealed record EditReviewCommand(int BusinessId, EditReviewDto Data) : ICommand;

internal sealed class EditReviewCommandHandler(InterconDbContext context) : ICommandHandler<EditReviewCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(EditReviewCommand command, CancellationToken cancellationToken)
    {
       var review = await _context.Reviews.FindAsync(command.BusinessId, command.Data.AuthorId);

        if (review == null)
        {
            return;
        }

        if (command.Data.Grade != 0)
        {
            review.Grade = command.Data.Grade;
        }
        if (command.Data.ReviewText != null) 
        { 
            review.ReviewText = command.Data.ReviewText;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class EditReviewCommandValidator : AbstractValidator<EditReviewCommand>
{
    public EditReviewCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty()
            .WithName(x => nameof(x.BusinessId));

        RuleFor(x => x.Data.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.Data.AuthorId));

        When(x => x.Data.Grade > 0, () =>
        {
            RuleFor(x => x.Data.Grade)
            .InclusiveBetween(1, 5)
            .WithName(x => nameof(x.Data.Grade));
        });

        When(x => x.Data.ReviewText is not null, () =>
        {
            RuleFor(x => x.Data.ReviewText)
            .MaximumLength(1000)
            .WithName(x => nameof(x.Data.ReviewText));
        });
    }
}