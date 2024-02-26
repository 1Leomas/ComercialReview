using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.User;

public record RegisterUserDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string? UserName { get; init; } = null;
    public CreateImageDto? Avatar { get; init; } = null;

    public Role Role { get; init; }
}