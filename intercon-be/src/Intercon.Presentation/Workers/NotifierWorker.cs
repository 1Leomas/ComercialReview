using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Notifications;
using Intercon.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;

namespace Intercon.Presentation.Workers;

public class NotifierWorker(
        IHubContext<NotificationHub, INotificationHub> context,
        IItemQueueService<Notification> notifications,
        ILogger<NotifierWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            var notification = await notifications.DequeueAsync(stoppingToken);

            if (notification is null) continue;

            var dateTime = DateTime.Now;

            logger.LogInformation("Executing {service} at {dateTime}", nameof(NotifierWorker), dateTime);

            await context.Clients.User(notification.UserId.ToString())
                .SendNotificationAsync(notification.Message);
        }
    }
}