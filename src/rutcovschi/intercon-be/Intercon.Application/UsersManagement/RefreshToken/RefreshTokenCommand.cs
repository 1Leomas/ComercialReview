using System.Security.Claims;
using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.UsersManagement.LoginUser;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Infrastructure.Identity;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.RefreshToken;

public record RefreshTokenCommand(Tokens Tokens) : ICommand<Tokens>;

internal sealed class RefreshTokenCommandHandler(
    ITokenService tokenService,
    UserManager<User> userManager,
    InterconDbContext context) : ICommandHandler<RefreshTokenCommand, Tokens>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<User> _userManager = userManager;
    private readonly InterconDbContext _context = context;

    public async Task<Tokens> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(command.Tokens.AccessToken);

        var userEmail = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        //var userEmail = principal.Claims.FirstOrDefault(x => x.Type == JwtClaimType.Email)?.Value;

        if (userEmail is null)
        {
            throw new InvalidOperationException("Unauthorized");
        }

        var savedRefreshToken = await _context.UserRefreshToken.
            FirstOrDefaultAsync(x => 
                x.UserEmail == userEmail && 
                x.RefreshToken == command.Tokens.RefreshToken && 
                x.IsActive == true, cancellationToken);

        if (savedRefreshToken is null)
        {
            throw new InvalidOperationException("Invalid refresh token");
        }

        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user == null)
        {
            throw new InvalidOperationException("Unauthorized");
        }

        var newJwtToken = _tokenService.CreateToken(user);

        if (newJwtToken is null)
        {
            throw new InvalidOperationException("Invalid attempt!");
        }

        var userRefreshToken = new UserRefreshTokens()
        {
            RefreshToken = newJwtToken.RefreshToken,
            UserEmail = user.Email!
        };

        _context.UserRefreshToken.Remove(savedRefreshToken);
        await _context.UserRefreshToken.AddAsync(userRefreshToken, cancellationToken);

        return newJwtToken;
    }
}