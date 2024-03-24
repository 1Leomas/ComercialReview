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
}