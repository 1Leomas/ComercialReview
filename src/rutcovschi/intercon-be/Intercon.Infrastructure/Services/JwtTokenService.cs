using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Intercon.Application.Abstractions;
using Intercon.Application.Options;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtTokenSettings> jwtTokenSettings, ILogger<JwtTokenService> logger) : ITokenService
{
    private readonly ILogger<JwtTokenService> _logger = logger;
    private readonly JwtTokenSettings _jwtTokenSettings = jwtTokenSettings.Value;


    public Tokens CreateTokens(int userId, int userRole)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpirationTimeInMinutes);

        var token = CreateJwtToken(
            CreateClaims(userId, userRole),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("JWT Token created");

        var accessToken = tokenHandler.WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(
            _jwtTokenSettings.ValidIssuer,
            _jwtTokenSettings.ValidAudience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private IEnumerable<Claim> CreateClaims(int userId, int userRole)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtClaimType.UserId, userId.ToString()),
                new(JwtClaimType.Role, userRole.ToString())
            };

            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = _jwtTokenSettings.SymmetricSecurityKey;

        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
            SecurityAlgorithms.HmacSha256
        );
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_jwtTokenSettings.SymmetricSecurityKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}