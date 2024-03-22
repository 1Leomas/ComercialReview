using Intercon.Domain.Entities;
using System.Security.Claims;
using Intercon.Domain.Identity;

namespace Intercon.Application.Abstractions;

public interface ITokenService
{
    Tokens CreateTokens(int userId, int userRole);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}