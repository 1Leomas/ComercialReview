using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Identity;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Intercon.Infrastructure.Services;

public class IdentityService(
    InterconDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService) : IIdentityService
{
    public async Task<Tokens> LoginUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        var managedUser = await userManager.FindByEmailAsync(email)
            ?? throw new InvalidOperationException("Bad credentials");

        var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, password);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Bad credentials");
        }

        var userInDb = context.Users.FirstOrDefault(u => u.Email == email) 
            ?? throw new InvalidOperationException("User not found");


        var tokens = tokenService.CreateTokens(userInDb);

        var savedRefreshToken = await context.UserRefreshToken.
            FirstOrDefaultAsync(x =>
                x.UserEmail == userInDb.Email &&
                x.IsActive == true, cancellationToken);

        if (savedRefreshToken is not null)
        {
            context.UserRefreshToken.Remove(savedRefreshToken);
        }

        var userRefreshToken = new UserRefreshTokens()
        {
            RefreshToken = tokens.RefreshToken,
            UserEmail = userInDb.Email!
        };

        context.UserRefreshToken.Add(userRefreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return tokens;
    }

    public async Task<Tokens> RefreshTokenAsync(Tokens tokens, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);

        var userEmail = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //var userEmail = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimType.Email)?.Value;

        if (userEmail is null)
        {
            throw new InvalidOperationException("Unauthorized");
        }

        var savedRefreshToken = await context.UserRefreshToken.FirstOrDefaultAsync(
            x => x.UserEmail == userEmail && x.RefreshToken == tokens.RefreshToken && x.IsActive == true, 
            cancellationToken) ?? throw new InvalidOperationException("Invalid refresh token");


        var user = await userManager.FindByEmailAsync(userEmail) 
            ?? throw new InvalidOperationException("User not found");

        var newJwtToken = tokenService.CreateTokens(user) 
            ?? throw new InvalidOperationException("Invalid attempt to create token");

        var userRefreshToken = new UserRefreshTokens()
        {
            RefreshToken = newJwtToken.RefreshToken,
            UserEmail = user.Email!
        };

        context.UserRefreshToken.Remove(savedRefreshToken);
        await context.UserRefreshToken.AddAsync(userRefreshToken, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return newJwtToken;
    }
}