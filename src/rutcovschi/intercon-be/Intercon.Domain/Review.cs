using Intercon.Domain.Enums;

namespace Intercon.Domain;

public class Review
{
    public int Id { get; set; }

    public User Author { get; set; }
    public string AuthorFullName => $"{Author.FirstName} {Author.LastName}";
    public string AuthorEmail => Author.Email;

    public float Rating { get; set; }
    public ReviewStatus RatingStatus { get; set; }

    public string ReviewTitle { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public bool WasEdited { get; set; }
}