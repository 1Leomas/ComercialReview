using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Identity;

namespace Intercon.Application.UsersManagement.LoginUser;

public record UserLoginResponse(Tokens Tokens);

public sealed record LoginUserCommand(LoginUserDto Data) : ICommand<Tokens>;

public sealed class LoginUserCommandHandler(IIdentityService identityService, IUserRepository userRepository)
    : ICommandHandler<LoginUserCommand, Tokens>
{
    public async Task<Tokens> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var userId = await userRepository.GetUserIdByEmailAsync(command.Data.Email, cancellationToken);

        if (userId == 0) throw new InvalidOperationException($"User with email {command.Data.Email} not found");

        var tokens = await identityService.LoginUserAsync(
            command.Data.Email,
            command.Data.Password,
            cancellationToken);

        return tokens;
    }
}