namespace Intercon.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;

    public int BusinessId { get; set; }
    public int ReviewAuthorId { get; set; }
    public int AuthorId { get; set; }

    public virtual Business Business { get; set; } = null!;
    public virtual Review Review { get; set; } = null!;
    public virtual User Author { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime UpdatedDate { get; set; } = DateTime.Now;
    public bool WasEdited => UpdatedDate != CreatedDate;
}