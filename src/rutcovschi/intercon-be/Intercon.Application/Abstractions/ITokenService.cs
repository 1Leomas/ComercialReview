using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Intercon.Application.Options;
using Intercon.Infrastructure.Identity;
using Microsoft.Extensions.Options;

namespace Intercon.Application.Abstractions;

public interface ITokenService
{
    Tokens CreateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

public class JwtTokenService(IOptions<JwtTokenSettings> jwtTokenSettings, ILogger<JwtTokenService> logger) : ITokenService
{
    private readonly ILogger<JwtTokenService> _logger = logger;
    private readonly JwtTokenSettings _jwtTokenSettings = jwtTokenSettings.Value;


    public Tokens CreateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtTokenSettings.ExpirationTimeInMinutes);

        var token = CreateJwtToken(
            CreateClaims(user),
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

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(
            _jwtTokenSettings.ValidIssuer,
            _jwtTokenSettings.ValidAudience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private List<Claim> CreateClaims(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtClaimType.UserId, user.Id.ToString()),
                new(JwtClaimType.Email, user.Email!),
                new(JwtClaimType.FirstName, user.FirstName),
                new(JwtClaimType.LastName, user.LastName),
                new(JwtClaimType.Role, ((int)user.Role).ToString())
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
        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
