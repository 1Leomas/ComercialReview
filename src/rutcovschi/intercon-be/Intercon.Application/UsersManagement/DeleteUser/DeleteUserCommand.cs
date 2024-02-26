using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand;

public sealed class DeleteUserCommandHandler(InterconDbContext context, UserManager<User> userManager) : ICommandHandler<DeleteUserCommand>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (userDb == null) 
        { 
            return; 
        }

        await _context.Images.Where(x => x.Id == userDb.AvatarId).ExecuteDeleteAsync(cancellationToken);
        await userManager.Users.Where(x => x.Id == userDb.Id).ExecuteDeleteAsync(cancellationToken);
    }
}
