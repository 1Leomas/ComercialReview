namespace Intercon.Application.Options;

public class JwtTokenSettings
{
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public string SymmetricSecurityKey { get; set; } = string.Empty;
    public int ExpirationTimeInMinutes { get; set; }
}