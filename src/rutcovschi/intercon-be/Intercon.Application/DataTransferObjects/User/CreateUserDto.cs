using Intercon.Domain;

namespace Intercon.Application.DataTransferObjects.User;

public record CreateUserDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; }
    public string? UserName { get; init; } = null;
}


