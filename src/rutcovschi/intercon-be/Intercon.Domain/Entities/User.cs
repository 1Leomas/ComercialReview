using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Domain.Entities;

public class User : IdentityUser<int>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Role Role { get; set; }

    public int? AvatarId { get; set; }

    public Image? Avatar { get; set; } = null;
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual Business Business { get; set; } = null!;

    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public bool WasEdited { get; set; }
}
