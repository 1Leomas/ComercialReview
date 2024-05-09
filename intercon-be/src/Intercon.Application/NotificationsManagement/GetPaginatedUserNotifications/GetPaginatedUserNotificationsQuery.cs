using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Notifications;
using Intercon.Domain.Pagination;

namespace Intercon.Application.NotificationsManagement.GetPaginatedUserNotifications;

public sealed record GetPaginatedUserNotificationsQuery(int UserId, NotificationParameters Parameters) : IQuery<PaginatedResponse<NotificationDto>>;

internal sealed class GetPaginatedUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    : IQueryHandler<GetPaginatedUserNotificationsQuery, PaginatedResponse<NotificationDto>>
{
    public async Task<PaginatedResponse<NotificationDto>> Handle(GetPaginatedUserNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository
            .GetPaginatedUserNotificationsAsync(query.UserId, query.Parameters, cancellationToken);

        return new PaginatedResponse<NotificationDto>
        {
            CurrentPage = notifications.CurrentPage,
            TotalPages = notifications.TotalPages,
            PageSize = notifications.PageSize,
            TotalCount = notifications.TotalCount,
            HasPrevious = notifications.HasPrevious,
            HasNext = notifications.HasNext,
            Items = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                DateTime = n.CreatedDate,
                IsRead = n.IsRead
            }).ToList()
        };
    }
}