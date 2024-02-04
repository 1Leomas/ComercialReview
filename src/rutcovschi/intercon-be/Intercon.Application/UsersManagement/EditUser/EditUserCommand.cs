using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.Users.EditUser;

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

        if (!command.FirstName.IsNullOrEmpty() )
        {
            if (command.FirstName!.Length > 50)
            {
                throw new ValidationException("Firstname length to long");
            }
            userDb.FirstName = command.FirstName;
        }

        if (!command.LastName.IsNullOrEmpty())
        {
            if (command.LastName!.Length > 50)
            {
                throw new ValidationException("Lastname length to long");
            }
            userDb.LastName = command.LastName;
        }

        if (!command.Email.IsNullOrEmpty())
        {
            if (!(await EmailUniqCheck(command.UserId, command.Email)))
            {
                throw new ValidationException("The email must be unique");
            }

            if (!ValidateEmail(command.Email))
            {
                throw new ValidationException("Bad email");
            }

            userDb.Email = command.Email;
        }

        if (!command.UserName.IsNullOrEmpty())
        {
            if (command.UserName!.Length > 50 || command.UserName!.Length < 2)
            {
                throw new ValidationException("Username bad length");
            }
            userDb.UserName = command.UserName;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<bool> EmailUniqCheck(int userId, string email)
    {
        return await _context.Users.AllAsync(x => x.Id == userId || x.Email != email);
    }

    private static bool ValidateEmail(string email)
    { 
        var emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
    }
}