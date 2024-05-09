using Intercon.Domain.Enums;
using Intercon.Domain.Notifications;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Domain.Entities;

public class User : IdentityUser<int>
{
    public User()
    {
        CreatedDate = UpdatedDate = DateTime.Now;
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override string Email { get; set; } = null!;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public Role Role { get; set; }

    public int? AvatarId { get; set; }

    public FileData? Avatar { get; set; } = null;
    public virtual Business Business { get; set; } = null!;
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<ReviewLike> ReviewLikes { get; set; } = new List<ReviewLike>();
    public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool WasEdited => UpdatedDate != CreatedDate;
}