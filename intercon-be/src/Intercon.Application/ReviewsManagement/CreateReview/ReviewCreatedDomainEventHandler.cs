using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.Abstractions.Services;
using Intercon.Domain.Events;
using Intercon.Domain.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.ReviewsManagement.CreateReview;

internal sealed class ReviewCreatedDomainEventHandler(
    IItemQueueService<Notification> queue,
    IBusinessRepository businessRepository,
    IUserRepository userRepository,
    INotificationRepository notificationRepository,
    ILogger<ReviewCreatedDomainEventHandler> logger) : INotificationHandler<ReviewCreatedDomainEvent>
{
    public async Task Handle(ReviewCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetBusinessByIdAsync(domainEvent.BusinessId, cancellationToken);

        if (business is null)
        {
            throw new InvalidOperationException("Business not found.");
        }

        var user = await userRepository.GetUserByIdAsync(domainEvent.AuthorId, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var notification = new Notification
        {
            UserId = business.OwnerId,
            Message = $"New {nameof(NotificationTypes.Review)} from {user.FirstName} {user.LastName}",
            CreatedDate = domainEvent.DateTime,
            UpdatedDate = domainEvent.DateTime,
            NotificationType = NotificationTypes.Review
        };

        queue.Enqueue(notification);

        var result = await notificationRepository.AddAsync(notification, cancellationToken);

        if (!result)
        {
            logger.LogError("Failed to save notification to the db.");
        }
    }
}