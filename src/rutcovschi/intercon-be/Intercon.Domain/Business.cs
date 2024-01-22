namespace Intercon.Domain;

public class Business
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public float RatingAvg { get; set; }
    public string Image { get; set; } //to do: trebuie de clarificat data type
    public string Address { get; set; }
    public uint ReviewsCount { get; set; }
    //public List<int, int> AllReviewsCount { get; set; }
    public Review Reviews { get; set; }
}