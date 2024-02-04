using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Application.UsersManagement.CreateUser;

public sealed record CreateUserCommand : ICommand
{
    public CreateUserCommand(CreateUserDto user)
    {
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Password = user.Password;
        UserName = user.UserName;
    }

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? UserName { get; init; }

}

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly InterconDbContext _context;

    public CreateUserCommandHandler(InterconDbContext context)
    {
        _context = context;
    }

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            UserName = command.UserName,
            Password = command.Password,
            Role = UserRole.User
        };

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}