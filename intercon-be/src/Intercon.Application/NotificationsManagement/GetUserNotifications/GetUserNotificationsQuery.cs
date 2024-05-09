using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.Notifications;

namespace Intercon.Application.NotificationsManagement.GetUserNotifications;

public sealed record GetUserNotificationsQuery(int UserId) : IQuery<List<NotificationDto>>;

internal sealed class GetUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    : IQueryHandler<GetUserNotificationsQuery, List<NotificationDto>>
{
    public async Task<List<NotificationDto>> Handle(GetUserNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByUserIdAsync(query.UserId, cancellationToken);

        return notifications.Select(n => new NotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            DateTime = n.CreatedDate,
            IsRead = n.IsRead
        }).ToList();
    }
}