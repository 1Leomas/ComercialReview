using Intercon.Domain.Enums;

namespace Intercon.Domain;

public class Review
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int AuthorId { get; set; }   

    public virtual User Author { get; set; }
    public float Rating { get; set; }
    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; }
    public bool WasEdited { get; set; }
}