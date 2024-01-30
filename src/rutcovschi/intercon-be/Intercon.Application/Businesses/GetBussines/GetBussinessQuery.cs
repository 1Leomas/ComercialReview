using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain;

namespace Intercon.Application.Businesses.GetBussines;

public record GetBussinessQuery(int Id) : IQuery
{
}


public record BusinessDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public float RatingAvg { get; set; }
    public string Image { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public uint ReviewsCount { get; set; }
    //public List<int, int> AllReviewsCount { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>(); // virtual for lazy loading
}
