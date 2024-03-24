using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand;

internal sealed class DeleteUserCommandHandler(IUserRepository userRepository, IFileRepository fileRepository) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var avatarId = await userRepository.GetAvatarIdIfExistsAsync(command.Id, cancellationToken);

        var result = await userRepository.DeleteUserAsync(command.Id, cancellationToken);

        if (result && avatarId.HasValue)
            await fileRepository.DeleteFileAsync(avatarId.Value, cancellationToken);
    }
}