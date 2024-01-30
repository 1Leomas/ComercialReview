using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public sealed class User : Entity
{
    public User(string firstName, string lastName, string password, string email, string? userName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        UserName = userName;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string? UserName { get; private set; } = null;
}
