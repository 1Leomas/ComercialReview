using Intercon.Application.CustomExceptions;
using Intercon.Application.ReviewsManagement.CreateReview;
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

    [ProducesResponseType(typeof(ReviewDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllReviews(CancellationToken cancellationToken)
    {
        var reviews = await mediator.Send(new GetAllReviewsQuery(), cancellationToken);


        return Ok(reviews);
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

    [HttpPost("api/businesses/{businessId}/reviews/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReview([FromRoute] int businessId, [FromRoute] int userId, [FromBody] CreateReviewDto reviewToAdd, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateReviewCommand(businessId,  userId, reviewToAdd), cancellationToken);

        return Ok();
    }

    //[HttpPut("{id}")]
    //[ProducesResponseType(typeof(ReviewDetailsDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> EditReview([FromRoute] int id, [FromBody] EditReviewDto reviewToEdit)
    //{
    //    await mediator.Send(new EditReviewCommand(id, reviewToEdit));

    //    return Ok();
    //}

    //[HttpDelete("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<IActionResult> DeleteReview(int id)
    //{
    //    await mediator.Send(new DeleteReviewCommand(id));

    //    return Ok();
    //}
}