namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { }

    public int Id { get; protected set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public DateTime UpdateDate { get; set; } = DateTime.Now;
    public bool WasEdited { get; set; }
}