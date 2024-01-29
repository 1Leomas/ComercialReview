using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Application.Users.CreateUser;    

public sealed record CreateUserCommand : ICommand<int>
{
    public CreateUserCommand(CreateUserDto userDto) 
    {
        FirstName = userDto.FirstName;
        LastName = userDto.LastName;
        Email = userDto.Email;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
}


public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
{
    private readonly InterconDbContext _context;

    public CreateUserCommandHandler(InterconDbContext context)
    {
        _context = context;
    }
                
    public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = new User(command.FirstName, command.LastName, command.Email);

        await _context.Users.AddAsync(userDb, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return userDb.Id;
    }
}
