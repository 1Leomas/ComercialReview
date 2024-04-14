using Intercon.Application.CommentsManagement.GetBusinessReviewComments;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Domain.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[ApiController]
public class CommentController(IMediator mediator) : ControllerBase
{
    [HttpGet("api/comments/{id}")]
    public IActionResult GetComment(int id)
    {
        return Ok();
    }

    [HttpGet("api/businesses/{businessId}/reviews/{userId}/comments")]
    [ProducesResponseType(typeof(PaginatedResponse<CommentDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedComments([FromRoute] int businessId, [FromRoute] int userId, [FromQuery] CommentParameters parameters, CancellationToken cancellationToken)
    {
        var paginatedComments = await mediator.Send(
            new GetPaginatedBusinessReviewCommentsQuery(businessId, userId, parameters), 
            cancellationToken);

        return Ok(paginatedComments);
    }

    [HttpPost("api/businesses/{businessId}/reviews/{userId}/comments")]
    public IActionResult AddComment()
    {
        return Ok();
    }

    [HttpPut("api/businesses/{businessId}/reviews/{userId}/comments{id}")]
    public IActionResult EditComment(int id)
    {
        return Ok();
    }

    [HttpDelete("api/businesses/{businessId}/reviews/{userId}/comments/{id}")]
    public IActionResult DeleteComment(int id)
    {
        return Ok();
    }
}