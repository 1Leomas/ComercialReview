using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class CommentLike : Entity, IEntity
{
    public int Id { get; init; }
    public int CommentId { get; set; }
    public int UserId { get; set; }

    public virtual Comment Comment { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}