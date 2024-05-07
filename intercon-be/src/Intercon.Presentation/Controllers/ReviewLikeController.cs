using Intercon.Application.CustomExceptions;
using Intercon.Application.ReviewsManagement.AddLike;
using Intercon.Application.ReviewsManagement.DeleteLike;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;


[ApiController]
public class ReviewLikeController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpPost("api/businesses/{businessId}/reviews/{userId}/like")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(
        [FromRoute] int businessId,         
        [FromRoute] int userId, 
        CancellationToken cancellationToken)     
    {
        var currentUserId = HttpContext.User.GetUserId();

        var res = await mediator.Send(new AddReviewLikeCommand(businessId, userId, currentUserId), cancellationToken);

        return Ok(res);
    }

    [Authorize]
    [HttpDelete("api/businesses/{businessId}/reviews/{userId}/like")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] int businessId,
        [FromRoute] int userId,
        CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var res = await mediator.Send(new DeleteReviewLikeCommand(businessId, userId, currentUserId), cancellationToken);

        return Ok(res);
    }
}