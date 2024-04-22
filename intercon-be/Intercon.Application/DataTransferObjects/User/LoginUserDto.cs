namespace Intercon.Application.DataTransferObjects.User;

public record LoginUserDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}