using Intercon.Application.CommentsManagement.AddLike;
using Intercon.Application.CommentsManagement.DeleteLike;
using Intercon.Application.CustomExceptions;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/review-comments/{id}/like")]
[ApiController]
public class CommentLikeController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromRoute] int id, CancellationToken token)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var res = await mediator
            .Send(new AddCommentLikeCommand(id, currentUserId), token);

        return Ok(res);
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken token)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var res = await mediator
            .Send(new DeleteCommentLikeCommand(id, currentUserId), token);

        return Ok(res);
    }
}