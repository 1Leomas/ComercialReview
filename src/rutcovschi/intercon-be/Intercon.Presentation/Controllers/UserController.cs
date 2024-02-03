using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Users.CreateUser;
using Intercon.Application.Users.DeleteUser;
using Intercon.Application.Users.GetUser;
using Intercon.Application.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/users")]

[ApiController]
public class UserController : BaseController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _mediator.Send(new GetUserQuery(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        catch (Exception)
        {
            return InternalServerError();
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            return Ok(await _mediator.Send(new GetUsersQuery()));
        }
        catch (Exception)
        {
            return InternalServerError();
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userToAdd)
    {
        try
        {
            await _mediator.Send(new CreateUserCommand(userToAdd));

            return Ok();
        }
        catch (Exception)
        {
            return InternalServerError();
        }
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditUser([FromBody] UserDto userToEdit)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await _mediator.Send(new DeleteUserCommand(id));

            return Ok();
        }
        catch (Exception)
        {
            return InternalServerError();
        }
    }
}
