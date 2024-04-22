using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.Logout;

public sealed record LogoutCommand(int UserId) : ICommand;

internal sealed class LogoutCommandHandler(
    IIdentityService identityService) : ICommandHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await identityService.LogoutUserAsync(command.UserId, cancellationToken);
    }
}