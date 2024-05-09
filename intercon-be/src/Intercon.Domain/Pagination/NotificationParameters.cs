using Intercon.Domain.Notifications;

namespace Intercon.Domain.Pagination;

public class NotificationParameters : QueryStringParameters
{
    public NotificationSortBy SortBy { get; set; } = NotificationSortBy.IsRead;
    public SortingDirection? SortDirection { get; set; }
    public IEnumerable<NotificationTypes> FilterByType { get; set; } = new List<NotificationTypes>();
    public bool OnlyUnread { get; set; }
}