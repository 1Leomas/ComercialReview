using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Users;
using Intercon.Application.Users.CreateUser;
using Intercon.Application.Users.DeleteUser;
using Intercon.Application.Users.GetUser;
using Intercon.Application.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/users")]

[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await _mediator.Send(new GetUserQuery(id));

        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return Ok(await _mediator.Send(new GetUsersQuery()));
    }
        
    [HttpPost()]    
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto userToAdd)
    {
        var result = await _mediator.Send(new CreateUserCommand(userToAdd));
        return Ok(result);
    }   

    [HttpPut()]
    public async Task<ActionResult> EditUser([FromBody] UserDto userToEdit)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUser([FromRoute] int id)
    {
        var result = await _mediator.Send(new DeleteUserCommand(id));

        if (!result)
        {
            return NotFound();
        }
            
        return Ok(result);
    }
}
