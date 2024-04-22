using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.UsersManagement.DeleteUser;
using Intercon.Application.UsersManagement.EditUser;
using Intercon.Application.UsersManagement.GetUser;
using Intercon.Application.UsersManagement.GetUsers;
using Intercon.Application.UsersManagement.LoginUser;
using Intercon.Application.UsersManagement.RefreshToken;
using Intercon.Application.UsersManagement.RegisterUser;
using Intercon.Application.UsersManagement.UserNameUniqueCheck;
using Intercon.Domain.Identity;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Application.UsersManagement.ForgotPassword;
using Intercon.Application.UsersManagement.Logout;
using Intercon.Application.UsersManagement.ResetPassword;
using Intercon.Application.UsersManagement.VerifyPasswordResetCode;
using Microsoft.AspNetCore.Identity.Data;
using ResetPasswordRequest = Intercon.Application.DataTransferObjects.User.ResetPasswordRequest;

namespace Intercon.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IMediator mediator) : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] int id, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery(id), cancellationToken);

        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetUsersQuery(), cancellationToken));
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(Tokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto userToLogin, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginUserCommand(userToLogin), cancellationToken);

        return Ok(response);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userToAdd, CancellationToken cancellationToken)
    {
        await mediator.Send(new RegisterUserCommand(userToAdd), cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpPut("edit")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> EditUser([FromForm] EditUserDto userToEdit, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();
        
        int? newLogoId = null!;
        if (userToEdit.Avatar is not null)
        {
            var logoFileData = await mediator.Send(new UploadFileCommand(userToEdit.Avatar), cancellationToken);

            if (logoFileData is null)
                throw new InvalidOperationException("Can not upload logo");

            newLogoId = logoFileData.Id;
        }

        var updatedUser = await mediator.Send(new EditUserCommand(currentUserId, userToEdit, newLogoId), cancellationToken);

        return Ok(updatedUser);
    }

    [Authorize]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        await mediator.Send(new DeleteUserCommand(currentUserId), cancellationToken);

        return Ok();
    }

    [HttpGet("check-user-name")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UserNameUniqueCheck([FromQuery(Name = "nameToCheck")] string userName, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UserNameUniqueCheckQuery(userName), cancellationToken);

        return result ? Ok() : BadRequest();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(Tokens), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var authorizationHeader = HttpContext.Request.Headers["authorization"];
        string accessToken = string.Empty;
        if (authorizationHeader.ToString().StartsWith("Bearer"))
        {
            accessToken = authorizationHeader.ToString().Substring("Bearer ".Length).Trim();
        }

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        var tokens = new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = request.RefreshToken
        };

        var response = await mediator.Send(new RefreshTokenCommand(tokens), cancellationToken);

        return Ok(response);
    }

    [Authorize]
    [HttpGet("identity")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserIdentity(CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var user = await mediator.Send(new GetUserQuery(currentUserId), cancellationToken);

        return Ok(user);
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPasswordDto,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ForgotPasswordCommand(forgotPasswordDto.Email), cancellationToken);

        return Ok();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new ResetPasswordCommand(resetPasswordRequest), cancellationToken);

        return Ok();
    }

    [HttpGet("verify-reset-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyResetCode([FromQuery] string resetCode,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new VerifyPasswordResetCodeQuery(resetCode), cancellationToken);

        return result ? Ok() : BadRequest();
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        var currentUserId = HttpContext.User.GetUserId();

        await mediator.Send(new LogoutCommand(currentUserId), CancellationToken.None);

        return Ok();
    }

}
