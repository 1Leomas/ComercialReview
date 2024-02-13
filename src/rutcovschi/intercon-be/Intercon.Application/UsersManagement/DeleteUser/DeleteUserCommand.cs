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
        await _context.Users.Where(x => x.Id == command.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
