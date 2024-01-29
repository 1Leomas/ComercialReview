using Intercon.Application.Abstractions.Messaging;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.DeleteUser;    

public sealed record DeleteUserCommand(int Id) : ICommand<bool>;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly InterconDbContext _context;

    public DeleteUserCommandHandler(InterconDbContext context)
    {
        _context = context;
    }
                
    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var userToDelete = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (userToDelete == null)
        {
            return false;
        }

        _context.Users.Remove(userToDelete);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
