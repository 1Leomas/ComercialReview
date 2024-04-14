using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class Comment : Entity, IEntity
{
    public int Id { get; init; }
    public string Text { get; set; } = null!;

    public int BusinessId { get; set; }
    public int ReviewAuthorId { get; set; }
    public int AuthorId { get; set; }

    public virtual Business Business { get; set; } = null!;
    public virtual Review Review { get; set; } = null!;
    public virtual User Author { get; set; } = null!;
}