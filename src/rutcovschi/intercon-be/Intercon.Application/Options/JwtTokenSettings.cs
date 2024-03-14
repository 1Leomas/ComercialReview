namespace Intercon.Application.Options;

public class JwtTokenSettings
{
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public string SymmetricSecurityKey { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
}