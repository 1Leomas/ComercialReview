using Intercon.Application.CustomExceptions;
using Intercon.Application.ReviewsManagement.CreateReview;
using Intercon.Application.ReviewsManagement.DeleteReview;
using Intercon.Application.ReviewsManagement.EditReview;
using Intercon.Application.ReviewsManagement.GetAllReviews;
using Intercon.Application.ReviewsManagement.GetBusinessReviews;
using Intercon.Application.ReviewsManagement.GetReviewDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[ApiController]
public class ReviewController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("api/reviews")]
    [ProducesResponseType(typeof(IEnumerable<ReviewShortDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllReviews(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllReviewsQuery(), cancellationToken));
    }

    [HttpGet("api/businesses/{businessId}/reviews/{userId}")]
    [ProducesResponseType(typeof(ReviewDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReview([FromRoute] int businessId, [FromRoute] int userId, CancellationToken cancellationToken)
    {
        var review = await _mediator.Send(new GetReviewDetailsQuery(businessId, userId), cancellationToken);

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
        return Ok(await _mediator.Send(new GetBusinessReviewsQuery(businessId), cancellationToken));
    }

    [HttpPost("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReview([FromRoute] int businessId, [FromBody] CreateReviewDto reviewToAdd, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateReviewCommand(businessId, reviewToAdd), cancellationToken);

        return Ok();
    }

    [HttpPut("api/businesses/{businessId}/reviews")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditReview([FromRoute] int businessId, [FromBody] EditReviewDto reviewToEdit, CancellationToken cancellationToken)
    {
        await mediator.Send(new EditReviewCommand(businessId, reviewToEdit), cancellationToken);

        return Ok();
    }

    [HttpDelete("api/businesses/{businessId}/reviews/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteReview([FromRoute] int businessId, [FromRoute] int userId, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteReviewCommand(businessId, userId), cancellationToken);

        return Ok();
    }
}