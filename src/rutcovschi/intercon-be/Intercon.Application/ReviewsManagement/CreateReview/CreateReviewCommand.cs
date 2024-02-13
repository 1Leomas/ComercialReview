using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.UsersManagement.CreateUser;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.ReviewsManagement.CreateReview;

public sealed record CreateReviewDto(int AuthorId, int Grade, string? ReviewText);
public sealed record CreateReviewCommand(int BusinessId, CreateReviewDto Data) : ICommand;

internal sealed class CreateReviewCommandHandler(InterconDbContext context) : ICommandHandler<CreateReviewCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        if (await _context.Businesses.AllAsync(x => x.Id != command.BusinessId, cancellationToken)
            || await _context.Users.AllAsync(x => x.Id != command.Data.AuthorId, cancellationToken))
        {
            return;
        }

        var s = new string[]{ "3:3", "4:2" };

        s.Select(x => {
            var a = int.Parse(x.Split(':').First());
            var b = int.Parse(x.Split(':').Last());
            return a > b ? 3 : a < b ? 0 : 1;
        }).Sum();


        var review = new Review
        {
            BusinessId = command.BusinessId,
            AuthorId = command.Data.AuthorId,
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

        RuleFor(x => x.Data.AuthorId)
            .NotEmpty()
            .WithName(x => nameof(x.Data.AuthorId));

        RuleFor(x => new {x.BusinessId, x.Data.AuthorId })
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
