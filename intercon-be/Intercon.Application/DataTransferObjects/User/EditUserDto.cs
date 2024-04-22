using Microsoft.AspNetCore.Http;

namespace Intercon.Application.DataTransferObjects.User;

public record EditUserDto
{
    public string? FirstName { get; init; } = null!;
    public string? LastName { get; init; } = null!;
    public string? Email { get; init; } = null!;
    public string? UserName { get; init; } = null!;
    public IFormFile? Avatar { get; init; }
}