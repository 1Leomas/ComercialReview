using Intercon.Domain.Notifications;
using Intercon.Domain.Pagination;

namespace Intercon.Application.Abstractions.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(int notificationId, CancellationToken cancellationToken);
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<PaginatedList<Notification>> GetPaginatedUserNotificationsAsync(int userId, NotificationParameters parameters, CancellationToken cancellationToken);
    Task<bool> AddAsync(Notification notification, CancellationToken cancellationToken);
    Task MarkAsRead(Notification notification, CancellationToken cancellationToken);
}