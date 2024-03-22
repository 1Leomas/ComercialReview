namespace Intercon.Domain.Identity;

public class Tokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class UserRefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}