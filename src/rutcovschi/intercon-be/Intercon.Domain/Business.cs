using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public class Business : Entity
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public float RatingAvg { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public uint ReviewsCount { get; set; }
    //public List<int, int> AllReviewsCount { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } // virtual for lazy loading
}