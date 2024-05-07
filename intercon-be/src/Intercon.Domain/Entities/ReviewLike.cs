using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class ReviewLike : Entity, IEntity
{
    public int Id { get; init; }
    public int BusinessId { get; set; }
    public int ReviewAuthorId { get; set; }
    public int UserId { get; set; }

    public virtual Review Review { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}