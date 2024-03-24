using Microsoft.AspNetCore.Http;

namespace Intercon.Application.DataTransferObjects.User;

public record RegisterUserDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? UserName { get; init; } = null;
    public IFormFile? Avatar { get; init; } = null;

    public int Role { get; init; }
}