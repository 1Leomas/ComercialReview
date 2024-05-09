using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Notifications;
using Intercon.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;

namespace Intercon.Presentation.Workers;

public class NotifierWorker : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);
    private readonly IHubContext<NotificationHub, INotificationHub> _context;
    private readonly IItemQueueService<Notification> _notifications;
    private readonly ILogger<NotifierWorker> _logger;

    public NotifierWorker(IHubContext<NotificationHub, INotificationHub> context,
        IItemQueueService<Notification> notifications,
        ILogger<NotifierWorker> logger)
    {
        _context = context;
        _notifications = notifications;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            var notification = await _notifications.DequeueAsync(stoppingToken);

            if (notification is null) continue;

            var dateTime = DateTime.Now;

            _logger.LogInformation("Executing {service} at {dateTime}", nameof(NotifierWorker), dateTime);

            await _context.Clients.User(notification.UserId.ToString())
                .SendNotificationAsync(notification.Message);
        }
    }
}