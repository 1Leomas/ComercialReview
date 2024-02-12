using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.UsersManagement.CreateUser;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int Grade, string? ReviewText);
public sealed record CreateReviewCommand(int BusinessId, int AuthorId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler(InterconDbContext context) : ICommandHandler<CreateReviewCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        if (await _context.Businesses.AllAsync(x => x.Id != command.BusinessId, cancellationToken)
            || await _context.Users.AllAsync(x => x.Id != command.AuthorId, cancellationToken))
        {
            return;
        }

        var review = new Review
        {
            BusinessId = command.BusinessId,
            AuthorId = command.AuthorId,
            Grade = command.Data.Grade,
            ReviewText = command.Data.ReviewText
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CreateUserCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateUserCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty()
            .WithName(x => nameof(x.BusinessId));

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.AuthorId));

        RuleFor(x => new {x.BusinessId, x.AuthorId})
            .MustAsync(async (data, _) =>
            {
                var exists = await context.Reviews.AnyAsync(x => x.BusinessId == data.BusinessId && x.AuthorId == data.AuthorId);
                return !exists;
            })
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
