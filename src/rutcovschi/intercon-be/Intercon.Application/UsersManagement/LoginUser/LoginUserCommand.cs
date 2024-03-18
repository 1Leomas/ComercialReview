using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Identity;

namespace Intercon.Application.UsersManagement.LoginUser;

public record UserLoginResponse(Tokens Tokens);

public sealed record LoginUserCommand(LoginUserDto Data) : ICommand<Tokens>;

public sealed class LoginUserCommandHandler(IIdentityService identityService) 
    : ICommandHandler<LoginUserCommand, Tokens>
{
    public async Task<Tokens> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var tokens = await identityService.LoginUserAsync(
            command.Data.Email, 
            command.Data.Password, 
            cancellationToken);

        return tokens;

        //var validCredentials = await _context.UsersOld
        //    .AnyAsync(u => u.Email == command.Data.Email && u.Password == command.Data.Password, cancellationToken);
        
        //if (!validCredentials)
        //{
        //    throw new InvalidOperationException("Invalid credentials");
        //}

        // to do - create access token

        //return accessToken, refreshToken
    }
}