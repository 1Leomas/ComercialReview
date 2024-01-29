namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    public int Id { get; init; }

    protected Entity()
    {
    }
}