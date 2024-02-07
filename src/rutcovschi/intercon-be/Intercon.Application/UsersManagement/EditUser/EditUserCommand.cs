using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using Intercon.Domain.Entities;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand : ICommand
{
    public EditUserCommand(int userId, EditUserDto user)
    {
        UserId = userId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        UserName = user.UserName;
    }

    public int UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? UserName { get; init; }
}

public sealed class EditUserCommandHandler : ICommandHandler<EditUserCommand>
{
    private readonly InterconDbContext _context;

    public EditUserCommandHandler(InterconDbContext context) => _context = context;

    public async Task Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

        if (userDb == null)
        {
            return;
        }

        if (!command.FirstName.IsNullOrEmpty())
        {
            userDb.FirstName = command.FirstName;
        }
        if (!command.LastName.IsNullOrEmpty())
        {
            userDb.LastName = command.LastName;
        }
        if (!command.Email.IsNullOrEmpty())
        {
            userDb.Email = command.Email;
        }
        if (!command.UserName.IsNullOrEmpty())
        {
            userDb.UserName = command.UserName;
        }
        else
        {
            userDb.UserName = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator(InterconDbContext dbContext)
    {
        When(x => !string.IsNullOrEmpty(x.FirstName), () =>
        {
            RuleFor(x => x.FirstName).MaximumLength(50);
        });
        When(x => !string.IsNullOrEmpty(x.LastName), () =>
        {
            RuleFor(x => x.LastName).MaximumLength(50);
        });

        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MustAsync(async (email, ctx) => await dbContext.Users.AllAsync(x => x.Email != email, ctx)
                ).WithMessage("The email must be unique");
        });

        When(x => !string.IsNullOrEmpty(x.UserName), () =>
        {
            RuleFor(x => x.UserName).Length(2, 50);

            RuleFor(x => x).MustAsync(async (command, ctx) => 
                await dbContext.Users.AllAsync(
                    x => x.Id == command.UserId || x.UserName != command.UserName, ctx)
            ).WithName(x => nameof(x.UserName)).WithMessage("The username must be unique");
        });
    }
}