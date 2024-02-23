using Azure.Core;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Application.UsersManagement.CreateUser;

public sealed record CreateUserCommand(CreateUserDto Data) : ICommand;

public sealed class CreateUserCommandHandler(
    InterconDbContext context, 
    UserManager<ApplicationUser> userManager) : ICommandHandler<CreateUserCommand>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        var result = await _userManager.CreateAsync(
            new ApplicationUser 
            { 
                FirstName = command.Data.FirstName,
                LastName = command.Data.LastName,
                UserName = command.Data.Email,
                Email = command.Data.Email, 
                Role = command.Data.Role 
            }, 
            command.Data.Password
        );

        if (!result.Succeeded)
        {
            throw new Exception("Can not register ApplicationUser");
        }

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

        var userDb = new User()
        {
            FirstName = command.Data.FirstName,
            LastName = command.Data.LastName,
            Email = command.Data.Email,
            Password = command.Data.Password,
            UserName = command.Data.UserName,
            Role = command.Data.Role,
            AvatarId = avatar?.Id
        };

        await _context.UsersOld.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}