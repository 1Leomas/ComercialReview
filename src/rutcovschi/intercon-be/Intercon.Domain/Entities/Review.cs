using System.ComponentModel.DataAnnotations;
using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class Review
{
    [Range(1, 5)]
    public int Grade { get; set; }
    public string? ReviewText { get; set; } = null!;

    public int BusinessId { get; set; }
    public int AuthorId { get; set; }

    public virtual User Author { get; set; } = null!;
    public virtual Business Business { get; set; } = null!;

    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public bool WasEdited { get; set; }
}