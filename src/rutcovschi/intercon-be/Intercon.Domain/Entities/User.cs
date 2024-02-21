using Intercon.Domain.Abstractions;
using Intercon.Domain.Enums;

namespace Intercon.Domain.Entities;

public class User : Entity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? UserName { get; set; } = null!;
    public Role Role { get; set; }

    public int? AvatarId { get; set; }

    public Image? Avatar { get; set; } = null;
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual Business Business { get; set; } = null!;
}