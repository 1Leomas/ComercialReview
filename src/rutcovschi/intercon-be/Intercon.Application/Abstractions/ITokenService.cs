using Intercon.Domain.Entities;
using Microsoft.Extensions.Logging;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Intercon.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Intercon.Application.Abstractions;

public interface ITokenService
{
    string CreateToken(User user);
}

public class JwtTokenService(ILogger<JwtTokenService> logger) : ITokenService
{
    private const int ExpirationMinutes = 30;
    private readonly ILogger<JwtTokenService> _logger = logger;

    public string CreateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration);

        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("JWT Token created");

        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) => new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtClaimType.UserId, user.Id.ToString()),
                new(JwtClaimType.Email, user.Email),
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
        var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
            SecurityAlgorithms.HmacSha256
        );
    }
}
