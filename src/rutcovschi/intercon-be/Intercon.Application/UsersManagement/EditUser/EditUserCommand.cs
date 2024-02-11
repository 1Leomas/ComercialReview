using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand;

public sealed class EditUserCommandHandler(InterconDbContext context) : ICommandHandler<EditUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

        if (userDb == null)
        {
            return;
        }

        if (!command.Data.FirstName.IsNullOrEmpty())
        {
            userDb.FirstName = command.Data.FirstName;
        }
        if (!command.Data.LastName.IsNullOrEmpty())
        {
            userDb.LastName = command.Data.LastName;
        }
        if (!command.Data.Email.IsNullOrEmpty())
        {
            userDb.Email = command.Data.Email;
        }
        if (!command.Data.UserName.IsNullOrEmpty())
        {
            userDb.UserName = command.Data.UserName;
        }
        else
        {
            userDb.UserName = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}