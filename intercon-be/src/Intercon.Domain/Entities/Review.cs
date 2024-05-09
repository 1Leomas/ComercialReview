using Intercon.Domain.Abstractions;
using Intercon.Domain.Enums;
using Intercon.Domain.Events;
using System.ComponentModel.DataAnnotations;

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
    public virtual ICollection<ReviewLike> Likes { get; set; } = new List<ReviewLike>();

    public static Review Create(int businessId, int authorId, int grade, string? reviewText,
        RecommendationType recommendation)
    {
        var date = DateTime.Now;

        var review = new Review
        {
            BusinessId = businessId,
            AuthorId = authorId,
            Grade = grade,
            ReviewText = reviewText,
            Recommendation = recommendation,
            CreatedDate = date,
            UpdatedDate = date
        };

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(businessId, authorId, date));

        return review;
    }
}