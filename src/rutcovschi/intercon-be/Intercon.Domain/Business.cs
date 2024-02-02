using Intercon.Domain.Abstractions;

namespace Intercon.Domain;

public class Business : Entity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public float Rating { get; set; }
    public string Image { get; set; }
    public string Address { get; set; }
    public uint ReviewsCount { get; set; }
    //public List<int, int> AllReviewsCount { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // virtual for lazy loading
}