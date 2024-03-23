using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand;

public sealed class EditUserCommandHandler(
        IUserRepository userRepository)
    : ICommandHandler<EditUserCommand>
{
    public async Task Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        await userRepository.UpdateUserAsync(
            command.UserId,
            command.Data,
            cancellationToken);
    }
}