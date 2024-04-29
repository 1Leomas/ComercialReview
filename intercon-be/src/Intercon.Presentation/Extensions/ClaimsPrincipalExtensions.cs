using Intercon.Domain.Enums;
using System.Security.Claims;

namespace Intercon.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var authenticatedUserIdClaim = claimsPrincipal.FindFirstValue(JwtClaimType.UserId);

        if (authenticatedUserIdClaim is null)
        {
            throw new InvalidOperationException("User is not authenticated");
        }

        return int.Parse(authenticatedUserIdClaim);
    }
    public static Role GetUserRole(this ClaimsPrincipal claimsPrincipal)
    {
        var userRoleClaim = claimsPrincipal.FindFirstValue(JwtClaimType.Role);

        if (userRoleClaim is null)
        {
            throw new InvalidOperationException("User role not found");
        }

        return (Role)int.Parse(userRoleClaim);
    }
}