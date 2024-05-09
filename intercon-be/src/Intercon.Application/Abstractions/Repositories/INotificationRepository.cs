using Intercon.Domain.Notifications;

namespace Intercon.Application.Abstractions.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, CancellationToken cancellationToken);
    //Task<PaginatedList<Notification>> GetPaginatedUserNotificationsAsync(int userId, NotificationParameters parameters, CancellationToken cancellationToken);
    Task<bool> CreateNotificationAsync(Notification notification, CancellationToken cancellationToken);
    //Task DeleteNotificationAsync(int notificationId, CancellationToken cancellationToken);
    //Task<bool> NotificationExistsAsync(int notificationId, CancellationToken cancellationToken);
}