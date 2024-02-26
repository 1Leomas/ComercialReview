using Intercon.Application.Abstractions.Messaging;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand;

public sealed class DeleteUserCommandHandler(InterconDbContext context) : ICommandHandler<DeleteUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await _context.AspNetUsers.FirstOrDefaultAsync(x => x.Id == command.Id);

        if (userDb == null) 
        { 
            return; 
        }

        await _context.Images.Where(x => x.Id == userDb.AvatarId).ExecuteDeleteAsync(cancellationToken);
        await _context.AspNetUsers.Where(x => x.Id == userDb.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
