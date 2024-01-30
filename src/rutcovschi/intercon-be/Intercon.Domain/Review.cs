using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public class Review : Entity
{
    public int BusinessId { get; set; }
    public int AuthorId { get; set; }

    public virtual User Author { get; set; }
    public float Rating { get; set; }
    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime? UpdateDate { get; set; } = null;
    public bool WasEdited { get; set; }
}