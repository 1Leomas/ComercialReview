using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand;

public sealed class DeleteUserCommandHandler(IUserRepository userRepository) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await userRepository.DeleteUserAsync(command.Id, cancellationToken);
    }
}