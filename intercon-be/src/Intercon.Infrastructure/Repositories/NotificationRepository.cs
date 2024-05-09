using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Notifications;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class NotificationRepository(InterconDbContext context) : INotificationRepository
{
    public async Task<Notification?> GetByIdAsync(int notificationId, CancellationToken cancellationToken)
    {
        return await context.Notifications
            .FirstOrDefaultAsync(x => x.Id == notificationId, cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Notifications
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<Notification>> GetPaginatedUserNotificationsAsync(int userId, NotificationParameters parameters,
        CancellationToken cancellationToken)
    {
        var notifications = context.Notifications
            .Where(x => x.UserId == userId)
            .AsQueryable();

        notifications = ApplyFilter(notifications, parameters);

        notifications = ApplySort(notifications, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<Notification>.ToPagedList(
                       notifications,
                                  parameters.PageNumber,
                                  parameters.PageSize);
    }

    private IQueryable<Notification> ApplySort(IQueryable<Notification> notifications, NotificationSortBy sortBy, SortingDirection? direction)
    {
        return sortBy switch
        {
            NotificationSortBy.CreatedDate => notifications.OrderUsing(x =>
                x.CreatedDate, direction ?? SortingDirection.Descending),
            NotificationSortBy.IsRead => notifications.OrderUsing(x =>
                x.IsRead, direction ?? SortingDirection.Ascending),
            _ => notifications
        };
    }

    private IQueryable<Notification> ApplyFilter(IQueryable<Notification> notifications, NotificationParameters queryParameters)
    {
        if (queryParameters.OnlyUnread)
        {
            notifications = notifications.Where(x => x.IsRead == false);
        }

        if (queryParameters.FilterByType.Any())
        {
            notifications = notifications.Where(x =>
                queryParameters.FilterByType.Contains(x.NotificationType));
        }

        return notifications;
    }

    public async Task<bool> AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await context.Notifications.AddAsync(notification, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task MarkAsRead(Notification notification, CancellationToken cancellationToken)
    {
        notification.MarkAsRead();
        context.Notifications.Update(notification);
        await context.SaveChangesAsync(cancellationToken);
    }
}