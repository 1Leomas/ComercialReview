using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public class Review : Entity
{
    public int BusinessId { get; set; }
    public int AuthorId { get; set; }

    public virtual User Author { get; set; }
    public virtual Business Business { get; set; }
    public float Grade { get; set; }
    public string ReviewText { get; set; }
}