using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Notifications;
using System.Collections.Concurrent;

namespace Intercon.Infrastructure.Services;

public class NotificationsQueueService : IItemQueueService<Notification>
{
    private readonly ConcurrentQueue<Notification> _notifications = new();

    private readonly SemaphoreSlim _signal = new(0);

    public void Enqueue(Notification notification)
    {
        _notifications.Enqueue(notification);
        _signal.Release();
    }

    public async Task<Notification?> DequeueAsync(CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _notifications.TryDequeue(out var notification);
        return notification;
    }
}