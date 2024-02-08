using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Application.UsersManagement.CreateUser;

public sealed record CreateUserCommand(CreateUserDto userToCreate) : ICommand
{
    public CreateUserDto Data { get; init; } = userToCreate;
}

public sealed class CreateUserCommandHandler(InterconDbContext context) : ICommandHandler<CreateUserCommand>
{
    private readonly InterconDbContext _context = context;

    public async Task Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = command.Data.ToEntity();

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}