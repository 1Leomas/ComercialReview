using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Identity;
using Intercon.Infrastructure.Options;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DateTime = System.DateTime;

namespace Intercon.Infrastructure.Services;

public class IdentityService(
    InterconDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService,
    IOptions<ResetPasswordSettings> resetPasswordSettings) : IIdentityService
{
    private readonly ResetPasswordSettings _resetPasswordSettings = resetPasswordSettings.Value;

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

        var userRefreshTokenDb = await context.RefreshTokens.FirstOrDefaultAsync(
                x => x.UserId == userDb.Id,
                cancellationToken);

        if (userRefreshTokenDb != null)
        {
            userRefreshTokenDb.Token = tokens.RefreshToken;

            userRefreshTokenDb.UpdatedDate = DateTime.Now;
        }
        else
        {
            var userRefreshToken = new RefreshToken()
            {
                Token = tokens.RefreshToken,
                UserId = userDb.Id
            };

            context.RefreshTokens.Add(userRefreshToken);
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

        var refreshTokenFromDb = await context.RefreshTokens.FirstOrDefaultAsync(
            x => x.UserId == userId && 
                                     x.Token == tokens.RefreshToken && 
                                     x.IsActive == true, 
            cancellationToken);

        if (refreshTokenFromDb == null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        var userRole = await context.Users
                           .Where(x => x.Id == userId)
                           .Select(x => x.Role)
                           .FirstOrDefaultAsync(cancellationToken);

        var newJwtToken = tokenService.CreateTokens(userId, (int)userRole) 
            ?? throw new InvalidOperationException("Invalid attempt to create token");


        refreshTokenFromDb.Token = newJwtToken.RefreshToken;

        await context.SaveChangesAsync(cancellationToken);

        return newJwtToken;
    }

    public async Task LogoutUserAsync(int userId, CancellationToken cancellationToken)
    {
        await context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<string> GenerateResetPasswordCodeAsync(int userId)
    {
        var random = new Random();
        var resetPasswordCode = random.Next(100000, 999999).ToString();

        await context.ResetPasswordCodes.AddAsync(new ResetPasswordCode()
        {
            UserId = userId,
            Code = resetPasswordCode,
            ValidUntilDate = DateTime.Now + TimeSpan.FromMinutes(_resetPasswordSettings.CodeExpirationTimeInMinutes)
        });

        await context.SaveChangesAsync();

        return resetPasswordCode;
    }

    public async Task<bool> VerifyResetPasswordCode(int userId, string resetPasswordCode)
    {
        var code = await context.ResetPasswordCodes.FirstOrDefaultAsync(
            x => x.UserId == userId
            && x.Code == resetPasswordCode
            && x.ValidUntilDate > DateTime.Now);

        if (code == null) return false;

        context.ResetPasswordCodes.Remove(code);
        await context.SaveChangesAsync();

        return true;
    }
    public async Task<bool> VerifyResetPasswordCode(string resetPasswordCode)
    {
        var code = await context.ResetPasswordCodes.FirstOrDefaultAsync(
            x => x.Code == resetPasswordCode
                 && x.ValidUntilDate > DateTime.Now);

        return code != null;
    }
}