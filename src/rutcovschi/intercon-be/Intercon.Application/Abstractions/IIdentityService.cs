using Intercon.Domain.Identity;

namespace Intercon.Application.Abstractions;

public interface IIdentityService
{
    Task<Tokens> LoginUserAsync(string email, string password, CancellationToken cancellationToken);
    Task<Tokens> RefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken);
    Task LogoutUserAsync(int userId, CancellationToken cancellationToken);
}