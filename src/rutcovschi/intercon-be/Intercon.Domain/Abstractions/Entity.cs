namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { }

    public int Id { get; init; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public bool WasEdited { get; set; }
}