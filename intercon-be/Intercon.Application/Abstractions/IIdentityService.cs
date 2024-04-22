using Intercon.Domain.Entities;
using Intercon.Domain.Identity;

namespace Intercon.Application.Abstractions;

public interface IIdentityService
{
    Task<Tokens> LoginUserAsync(string email, string password, CancellationToken cancellationToken);
    Task<Tokens> RefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken);
    Task LogoutUserAsync(int userId, CancellationToken cancellationToken);
    Task<string> GenerateResetPasswordCodeAsync(int userId);
    Task<bool> VerifyResetPasswordCode(int userId, string resetPasswordCode);
    Task<bool> VerifyResetPasswordCode(string resetPasswordCode);
}