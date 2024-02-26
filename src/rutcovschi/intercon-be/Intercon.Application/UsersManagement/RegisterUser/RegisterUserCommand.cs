using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Application.UsersManagement.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserDto Data) : ICommand;

public sealed class RegisterUserCommandHandler(
    InterconDbContext context, 
    UserManager<User> userManager) : ICommandHandler<RegisterUserCommand>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        Image? avatar = null;

        if (command.Data.Avatar is not null)
        {
            avatar = new Image()
            {
                Data = command.Data.Avatar.Data
            };

            await _context.Images.AddAsync(avatar, cancellationToken);
            var rows = await _context.SaveChangesAsync(cancellationToken);

            if (rows == 0)
            {
                throw new Exception("Cannot add avatar");
            }
        }

        var result = await _userManager.CreateAsync(
            new User
            { 
                FirstName = command.Data.FirstName,
                LastName = command.Data.LastName,
                Email = command.Data.Email,
                UserName = command.Data.UserName ?? command.Data.Email,
                Role = command.Data.Role,
                AvatarId = avatar?.Id
            }, 
            command.Data.Password
        );

        if (!result.Succeeded)
        {
            throw new Exception("Can not register ApplicationUser");
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}