using Intercon.Application.CommentsManagement.AddComment;
using Intercon.Application.CommentsManagement.DeleteComment;
using Intercon.Application.CommentsManagement.EditComment;
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

[Route("api")]
[ApiController]
public class CommentController : ControllerBase
{
    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMediator _mediator;

    [HttpGet("comments/{id}")]
    public IActionResult GetComment(int id)
    {
        return Ok();
    }

    [HttpGet("businesses/{businessId}/reviews/{reviewAuthorId}/comments")]
    [ProducesResponseType(typeof(PaginatedResponse<CommentDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedComments([FromRoute] int businessId, [FromRoute] int reviewAuthorId, [FromQuery] CommentParameters parameters, CancellationToken cancellationToken)
    {
        var paginatedComments = await _mediator.Send(
            new GetPaginatedBusinessReviewCommentsQuery(businessId, reviewAuthorId, parameters), 
            cancellationToken);

        return Ok(paginatedComments);
    }

    [Authorize]
    [HttpPost("businesses/{businessId}/reviews/{reviewAuthorId}/comments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddComment([FromRoute] int businessId, [FromRoute] int reviewAuthorId, [FromBody] string text, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var comment = new AddCommentDto
        {
            BusinessId = businessId,
            ReviewAuthorId = reviewAuthorId,
            AuthorId = currentUserId,
            Text = text
        };

        await _mediator.Send(new AddCommentCommand(comment), cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpPut("review-comments/{id}")]
    public async Task<IActionResult> EditComment([FromRoute] int id, [FromBody] string text, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var comment = new EditCommentDto
        {
            Id = id,
            AuthorId = currentUserId,
            Text = text
        };

        await _mediator.Send(new EditCommentCommand(comment), cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpDelete("review-comments/{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] int id, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();
        var currentUserRole = HttpContext.User.GetUserRole();

        var deleteCommentRequest = new DeleteCommentRequest
        {
            Id = id,
            CurrentUserId = currentUserId,
            CurrentUserRole = currentUserRole
        };

        await _mediator.Send(new DeleteCommentCommand(deleteCommentRequest), cancellationToken);

        return Ok();
    }
}