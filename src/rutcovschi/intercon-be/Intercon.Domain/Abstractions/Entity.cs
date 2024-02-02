namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { }

    public int Id { get; protected set; }
    public DateTime CreateDate { get; protected set; }
    public DateTime UpdateDate { get; set; }
    public bool WasEdited { get; set; }
}