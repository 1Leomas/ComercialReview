using System.ComponentModel.DataAnnotations;
using Intercon.Domain.Abstractions;
using Intercon.Domain.Enums;

namespace Intercon.Domain.Entities;

public class Review : Entity
{
    [Range(1, 5)]
    public int Grade { get; set; }
    public string? ReviewText { get; set; } = null!;
    public RecommendationType Recommendation { get; set; }
    public uint CommentsCount { get; set; }

    public int BusinessId { get; set; }
    public int AuthorId { get; set; }

    public virtual User Author { get; set; } = null!;
    public virtual Business Business { get; set; } = null!;
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}