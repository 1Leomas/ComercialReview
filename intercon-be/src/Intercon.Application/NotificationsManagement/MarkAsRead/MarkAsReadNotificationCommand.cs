using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;

namespace Intercon.Application.NotificationsManagement.MarkAsRead;

public sealed record MarkAsReadNotificationCommand(int NotificationId, int UserId) : ICommand;

internal sealed class MarkAsReadNotificationCommandHandler(INotificationRepository notificationRepository)
    : ICommandHandler<MarkAsReadNotificationCommand>
{
    public async Task Handle(MarkAsReadNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(command.NotificationId, cancellationToken);

        if (notification is null || notification.UserId != command.UserId)
        {
            throw new InvalidOperationException("Notification not found");
        }

        await notificationRepository.MarkAsRead(notification, cancellationToken);
    }
}