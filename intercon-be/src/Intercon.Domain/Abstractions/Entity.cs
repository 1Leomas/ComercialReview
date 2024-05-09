namespace Intercon.Domain.Abstractions;

public abstract class Entity
{
    protected Entity()
    {
        CreatedDate = UpdatedDate = DateTime.Now;
    }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool WasEdited => UpdatedDate != CreatedDate;

    private readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> DomainEvents => _domainEvents.ToList();

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}