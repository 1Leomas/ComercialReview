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
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserQuery(id));

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _mediator.Send(new GetUsersQuery()));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userToAdd)
    {
        await _mediator.Send(new CreateUserCommand(userToAdd));

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditUser([FromBody] UserDetailsDto userToEdit)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));

        return Ok();
    }
}
