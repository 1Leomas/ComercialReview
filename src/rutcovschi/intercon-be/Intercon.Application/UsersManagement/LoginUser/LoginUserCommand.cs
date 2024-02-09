using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.LoginUser;

public sealed record LoginUserCommand(LoginUserDto userToLogin) : ICommand
{
    public LoginUserDto Data { get; init; } = userToLogin;
}

public sealed class LoginUserCommandHandler(InterconDbContext context) : ICommandHandler<LoginUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == command.Data.Email && u.Password == command.Data.Password, cancellationToken);
        
        if (userDb is null) throw new InvalidOperationException("Invalid credentials");
    }
}