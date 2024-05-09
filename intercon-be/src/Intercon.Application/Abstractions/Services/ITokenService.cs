using Intercon.Domain.Identity;
using System.Security.Claims;

namespace Intercon.Application.Abstractions.Services;

public interface ITokenService
{
    Tokens CreateTokens(int userId, int userRole);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}