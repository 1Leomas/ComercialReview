using Azure.Core;
using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.LoginUser;

public record UserLoginResponse(string Token);

public sealed record LoginUserCommand(LoginUserDto Data) : ICommand<UserLoginResponse>;

public sealed class LoginUserCommandHandler(
    InterconDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService) : ICommandHandler<LoginUserCommand, UserLoginResponse>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<UserLoginResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var managedUser = await _userManager.FindByEmailAsync(command.Data.Email) 
                          ?? throw new InvalidOperationException("Bad credentials");


        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, command.Data.Password!);

        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Bad credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(u => u.Email == command.Data.Email);

        if (userInDb is null)
        {
            throw new InvalidOperationException("Unauthorized");
        }

        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync(cancellationToken);

        return new UserLoginResponse(accessToken);


        //var validCredentials = await _context.UsersOld
        //    .AnyAsync(u => u.Email == command.Data.Email && u.Password == command.Data.Password, cancellationToken);
        
        //if (!validCredentials)
        //{
        //    throw new InvalidOperationException("Invalid credentials");
        //}

        // to do - create access token

        //return accessToken, refreshToken
    }
}