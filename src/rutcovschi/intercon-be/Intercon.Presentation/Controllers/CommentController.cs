using Intercon.Application.CommentsManagement.AddComment;
using Intercon.Application.CommentsManagement.GetBusinessReviewComments;
using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Comment;
using Intercon.Domain.Pagination;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[ApiController]
public class CommentController : ControllerBase
{
    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMediator _mediator;

    [HttpGet("api/comments/{id}")]
    public IActionResult GetComment(int id)
    {
        return Ok();
    }

    [HttpGet("api/businesses/{businessId}/reviews/{userId}/comments")]
    [ProducesResponseType(typeof(PaginatedResponse<CommentDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedComments([FromRoute] int businessId, [FromRoute] int userId, [FromQuery] CommentParameters parameters, CancellationToken cancellationToken)
    {
        var paginatedComments = await _mediator.Send(
            new GetPaginatedBusinessReviewCommentsQuery(businessId, userId, parameters), 
            cancellationToken);

        return Ok(paginatedComments);
    }

    [Authorize]
    [HttpPost("api/businesses/{businessId}/reviews/{userId}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddComment([FromRoute] int businessId, [FromRoute] int userId, [FromBody] string text, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var comment = new AddCommentDto
        {
            BusinessId = businessId,
            ReviewAuthorId = userId,
            AuthorId = currentUserId,
            Text = text
        };

        await _mediator.Send(new AddCommentCommand(comment), cancellationToken);

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