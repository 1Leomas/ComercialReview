namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    protected Entity()
    {
        CreatedDate = UpdatedDate = DateTime.UtcNow;
    }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool WasEdited => UpdatedDate != CreatedDate;
}