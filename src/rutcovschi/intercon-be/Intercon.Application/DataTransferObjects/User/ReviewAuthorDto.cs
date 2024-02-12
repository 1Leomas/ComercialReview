namespace Intercon.Application.DataTransferObjects.User;

public record ReviewAuthorDto(
    string FirstName,
    string LastName,
    string? UserName
);