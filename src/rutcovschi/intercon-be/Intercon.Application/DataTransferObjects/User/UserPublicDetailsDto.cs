namespace Intercon.Application.DataTransferObjects.User;

public record UserPublicDetailsDto
{
    public int Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? UserName { get; init; }
    public string? AvatarPath { get; init; }
}