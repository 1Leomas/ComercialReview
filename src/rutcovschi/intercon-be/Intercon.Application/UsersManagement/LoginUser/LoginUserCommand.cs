using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.LoginUser;

public sealed record LoginUserCommand(LoginUserDto Data) : ICommand;

public sealed class LoginUserCommandHandler(InterconDbContext context) : ICommandHandler<LoginUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var validCredentials = await _context.Users
            .AnyAsync(u => u.Email == command.Data.Email && u.Password == command.Data.Password, cancellationToken);
        
        if (!validCredentials)
        {
            throw new InvalidOperationException("Invalid credentials");
        }

        // to do - create access token

        //return accessToken, refreshToken
    }
}