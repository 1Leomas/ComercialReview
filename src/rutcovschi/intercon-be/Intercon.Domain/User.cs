using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public class User : Entity
{
    public User() { }

    public User(string firstName, string lastName, string password, string email, string? userName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        UserName = userName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? UserName { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
