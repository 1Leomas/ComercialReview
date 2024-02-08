namespace Intercon.Application.DataTransferObjects.User;

public record UserDetailsDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? UserName);