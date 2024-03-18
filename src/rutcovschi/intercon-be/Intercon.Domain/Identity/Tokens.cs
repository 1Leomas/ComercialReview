namespace Intercon.Domain.Identity;

public class Tokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class UserRefreshTokens
{
    public int Id { get; set; }

    public string UserEmail { get; set; } = null!;

    public string RefreshToken { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}