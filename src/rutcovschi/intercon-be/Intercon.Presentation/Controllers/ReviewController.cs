using Intercon.Application.CustomExceptions;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.DeleteReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Application.ReviewsManagement.GetAllReviews;
using Intercon.Application.ReviewsManagement.GetBusinessReviews;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[ApiController]
public class ReviewController(IMediator mediator) : BaseController
{
    [HttpGet("api/reviews")]
    [ProducesResponseType(typeof(IEnumerable<ReviewShortDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllReviews(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetAllReviewsQuery(), cancellationToken));
    }

    [HttpGet("api/businesses/{businessId}/reviews/{userId}")]
    [ProducesResponseType(typeof(ReviewDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReview([FromRoute] int businessId, [FromRoute] int userId, CancellationToken cancellationToken)
    {
        var review = await mediator.Send(new GetReviewDetailsQuery(businessId, userId), cancellationToken);

        if (review == null)
        {
            return NotFound();
        }

        return Ok(review);
    }

    [HttpGet("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(typeof(IEnumerable<ReviewDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinessReviews([FromRoute] int businessId, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetBusinessReviewsQuery(businessId), cancellationToken));
    }

    [Authorize]
    [HttpPost("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReview([FromRoute] int businessId, [FromBody] CreateReviewDto reviewToAdd, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        await mediator.Send(new CreateReviewCommand(businessId, currentUserId, reviewToAdd), cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpPut("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(typeof(UpdatedReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditReview([FromRoute] int businessId, [FromBody] EditReviewDto editReviewData, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var updatedReview = await mediator.Send(new EditReviewCommand(businessId, currentUserId, editReviewData), cancellationToken);

        return Ok(updatedReview);
    }

    [Authorize(Roles = "2")]
    [HttpDelete("api/businesses/{businessId}/reviews/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUserReview([FromRoute] int businessId, [FromRoute] int userId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteReviewCommand(businessId, userId), cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteCurrentUserReview([FromRoute] int businessId, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        await mediator.Send(new DeleteReviewCommand(businessId, currentUserId), cancellationToken);

        return Ok();
    }
}