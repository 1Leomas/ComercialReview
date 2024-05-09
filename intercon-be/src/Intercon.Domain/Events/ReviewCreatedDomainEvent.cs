using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Events;

public class ReviewCreatedDomainEvent : IDomainEvent
{
    public ReviewCreatedDomainEvent(int businessId, int authorId, DateTime dateTime)
    {
        BusinessId = businessId;
        AuthorId = authorId;
        DateTime = dateTime;
    }

    public int BusinessId;
    public int AuthorId;
    public DateTime DateTime;
}