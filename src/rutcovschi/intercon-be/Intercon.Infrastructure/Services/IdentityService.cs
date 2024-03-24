using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Identity;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Services;

public class IdentityService(
    InterconDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService) : IIdentityService
{
    public async Task<Tokens> LoginUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var userDb = await userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Bad credentials");

        var isPasswordValid = await userManager.CheckPasswordAsync(userDb, password);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Bad credentials");
        }

        var tokens = tokenService.CreateTokens(userDb.Id, (int)userDb.Role);

        var userRefreshTokenDb = await context.RefreshToken.FirstOrDefaultAsync(
                x => x.UserId == userDb.Id,
                cancellationToken);

        if (userRefreshTokenDb != null)
        {
            userRefreshTokenDb.Token = tokens.RefreshToken;

            userRefreshTokenDb.UpdateDate = DateTime.Now;
            userRefreshTokenDb.WasEdited = true;
        }
        else
        {
            var userRefreshToken = new RefreshToken()
            {
                Token = tokens.RefreshToken,
                UserId = userDb.Id
            };

            context.RefreshToken.Add(userRefreshToken);
        }

        await context.SaveChangesAsync(cancellationToken);

        return tokens;
    }

    public async Task<Tokens> RefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);

        var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimType.UserId)
            ?? throw new InvalidOperationException("Unauthorized");

        var userId = int.Parse(userIdClaim.Value);

        var refreshTokenFromDb = await context.RefreshToken.FirstOrDefaultAsync(
            x => x.UserId == userId && x.Token == tokens.RefreshToken && x.IsActive == true, 
            cancellationToken);

        if (refreshTokenFromDb == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        var userRole = await context.Users
                           .Where(x => x.Id == userId).Select(x => x.Role)
                           .FirstOrDefaultAsync(cancellationToken);

        var newJwtToken = tokenService.CreateTokens(userId, (int)userRole) 
            ?? throw new InvalidOperationException("Invalid attempt to create token");


        refreshTokenFromDb.Token = newJwtToken.RefreshToken;

        await context.SaveChangesAsync(cancellationToken);

        return newJwtToken;
    }
}