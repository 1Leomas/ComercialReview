using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Notifications;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class NotificationRepository(InterconDbContext context) : INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Notifications
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> CreateNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}