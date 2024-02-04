﻿namespace Intercon.Application.DataTransferObjects.User;

public record EditUserDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? UserName { get; init; } = null;
}