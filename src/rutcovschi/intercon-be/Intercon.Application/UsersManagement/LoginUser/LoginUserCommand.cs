using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Identity;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.LoginUser;

public record UserLoginResponse(Tokens Tokens);

public sealed record LoginUserCommand(LoginUserDto Data) : ICommand<Tokens>;

public sealed class LoginUserCommandHandler(
    InterconDbContext context,
    UserManager<User> userManager,
    ITokenService tokenService) : ICommandHandler<LoginUserCommand, Tokens>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Tokens> Handle(LoginUserCommand command, CancellationToken cancellationToken)
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

        var tokens = _tokenService.CreateToken(userInDb);

        var savedRefreshToken = await _context.UserRefreshToken.
            FirstOrDefaultAsync(x =>
                x.UserEmail == userInDb.Email &&
                x.IsActive == true, cancellationToken);

        if (savedRefreshToken is not null)
        {
            _context.UserRefreshToken.Remove(savedRefreshToken);
        }

        var userRefreshToken = new UserRefreshTokens()
        {
            RefreshToken = tokens.RefreshToken,
            UserEmail = userInDb.Email!
        };

        _context.UserRefreshToken.Add(userRefreshToken);

        await _context.SaveChangesAsync(cancellationToken);

        return tokens;


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