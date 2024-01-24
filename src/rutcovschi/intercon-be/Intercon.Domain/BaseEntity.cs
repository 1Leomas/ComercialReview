namespace Intercon.Domain;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; }
    public bool WasEdited { get; set; }
}