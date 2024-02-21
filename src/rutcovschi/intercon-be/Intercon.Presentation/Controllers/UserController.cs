using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.UsersManagement.CreateUser;
using Intercon.Application.UsersManagement.DeleteUser;
using Intercon.Application.UsersManagement.EditUser;
using Intercon.Application.UsersManagement.GetUser;
using Intercon.Application.UsersManagement.GetUsers;
using Intercon.Application.UsersManagement.LoginUser;
using Intercon.Application.UsersManagement.VerifyUserName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserQuery(id), cancellationToken);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetUsersQuery(), cancellationToken));
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto userToLogin, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new LoginUserCommand(userToLogin), cancellationToken);

        return Ok(response);
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userToAdd, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateUserCommand(userToAdd), cancellationToken);

        return Ok();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditUser([FromRoute] int id, [FromBody] EditUserDto userToEdit, CancellationToken cancellationToken)
    {
        await _mediator.Send(new EditUserCommand(id, userToEdit), cancellationToken);

        return Ok();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);

        return Ok();
    }

    [HttpGet("CheckUserName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserNameUniqueCheck([FromQuery(Name = "nameToCheck")] string userName, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UserNameUniqueCheckQuery(userName), cancellationToken);

        return result ? Ok() : BadRequest();
    }
}
