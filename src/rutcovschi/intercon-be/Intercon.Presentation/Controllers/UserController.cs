using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.UsersManagement.CreateUser;
using Intercon.Application.UsersManagement.DeleteUser;
using Intercon.Application.UsersManagement.EditUser;
using Intercon.Application.UsersManagement.GetUser;
using Intercon.Application.UsersManagement.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/users")]

[ApiController]
public class UserController(IMediator mediator) : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await mediator.Send(new GetUserQuery(id));

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
        return Ok(await mediator.Send(new GetUsersQuery()));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userToAdd)
    {
        await mediator.Send(new CreateUserCommand(userToAdd));

        return Ok();
    }

    [HttpPut]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditUser([FromQuery] int id, [FromBody] EditUserDto userToEdit)
    {
        await mediator.Send(new EditUserCommand(id, userToEdit));

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        await mediator.Send(new DeleteUserCommand(id));

        return Ok();
    }
}
