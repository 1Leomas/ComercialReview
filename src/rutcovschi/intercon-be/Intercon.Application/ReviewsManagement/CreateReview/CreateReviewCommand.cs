using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int AuthorId, int Grade, string? ReviewText);
public sealed record CreateReviewCommand(int BusinessId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler(InterconDbContext context) : ICommandHandler<CreateReviewCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var business = await _context.Businesses.FindAsync(command.BusinessId, cancellationToken);

        var review = new Review
        {
            BusinessId = command.BusinessId,
            AuthorId = command.Data.AuthorId,
            Grade = command.Data.Grade,
            ReviewText = command.Data.ReviewText
        };

        var reviews = _context.Reviews.Where(x => x.BusinessId == command.BusinessId);

        var reviewsCount = (uint)(reviews.Count() + 1);
        float reviewsSum = reviews.Select(x => x.Grade).Sum() + command.Data.Grade;

        business!.ReviewsCount = reviewsCount;
        business.Rating = reviewsSum / reviewsCount;

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CreateReviewCommandCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty()
            .WithName(x => nameof(x.BusinessId));

        RuleFor(x => x.BusinessId)
            .MustAsync(async (businessId, ctx) =>
            {
                return await context.Businesses.AnyAsync(x => x.Id == businessId, ctx);
            })
            .WithMessage("The business doesn't exists");

        RuleFor(x => x.Data.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.Data.AuthorId));

        RuleFor(x => x.Data.AuthorId)
            .MustAsync(async (authorId, ctx) =>
            {
                return await context.Users.AnyAsync(x => x.Id == authorId, ctx);
            })
            .WithMessage("The author doesn't exists");

        RuleFor(x => new {x.BusinessId, x.Data.AuthorId })
            .MustAsync(async (data, ctx) =>
            {
                var exists = await context.Reviews.AnyAsync(x => x.BusinessId == data.BusinessId && x.AuthorId == data.AuthorId, ctx);
                return !exists;
            })
            .WithName(x => nameof(x.Data.AuthorId))
            .WithMessage("The user already wrote a review");

        RuleFor(x => x.Data.Grade)
            .NotEmpty()
            .InclusiveBetween(1, 5)
            .WithName(x => nameof(x.Data.Grade));
        
        RuleFor(x => x.Data.ReviewText)
            .MaximumLength(1000)
            .WithName(x => nameof(x.Data.ReviewText));
    }
}
