using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Identity;

namespace Intercon.Application.UsersManagement.RefreshToken;

public record RefreshTokenCommand(Tokens Tokens) : ICommand<Tokens>;

internal sealed class RefreshTokenCommandHandler
    (IIdentityService identityService) : ICommandHandler<RefreshTokenCommand, Tokens>
{
    public async Task<Tokens> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var newJwtToken = await identityService
            .RefreshTokenAsync(command.Tokens, cancellationToken);

        return newJwtToken;
    }
}