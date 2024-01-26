using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        return Ok(await _userService.GetUser(id));
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        return Ok(await _userService.GetUsers());
    }

    [HttpPost()]
    public async Task<ActionResult> CreateUsers([FromBody] UserDto userToAdd)
    {
        return Ok(await _userService.CreateUser(userToAdd));
    }   

    [HttpPut()]
    public async Task<ActionResult> EditUser([FromBody] UserDto userToEdit)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUser([FromRoute] int id)
    {
        return Ok(await _userService.DeleteUser(id));
    }
}
