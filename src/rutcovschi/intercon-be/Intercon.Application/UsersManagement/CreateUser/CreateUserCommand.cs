using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Application.UsersManagement.CreateUser;

public sealed record CreateUserCommand(CreateUserDto Data) : ICommand;

public sealed class CreateUserCommandHandler(InterconDbContext context) : ICommandHandler<CreateUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
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
            AvatarId = avatar?.Id
        };

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}