using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Notifications;
using Intercon.Application.NotificationsManagement.GetPaginatedUserNotifications;
using Intercon.Application.NotificationsManagement.GetUserNotifications;
using Intercon.Application.NotificationsManagement.MarkAsRead;
using Intercon.Domain.Pagination;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByCurrentUser(CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        return Ok(await mediator.Send(new GetUserNotificationsQuery(currentUserId), cancellationToken));
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<NotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaginatedByCurrentUser([FromQuery] NotificationParameters parameters, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        return Ok(await mediator.Send(new GetPaginatedUserNotificationsQuery(currentUserId, parameters), cancellationToken));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarkAsRead([FromRoute] int id, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        await mediator.Send(new MarkAsReadNotificationCommand(id, currentUserId), cancellationToken);

        return Ok();
    }
}