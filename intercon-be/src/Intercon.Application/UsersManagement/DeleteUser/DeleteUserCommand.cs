using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.DeleteUser;

public sealed record DeleteUserCommand(int Id) : ICommand;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository, 
    IBlobStorage blobStorage,
    IFileRepository fileRepository) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var usedDb = await userRepository.GetUserByIdAsync(command.Id, cancellationToken);

        if (usedDb == null)
        {
            return;
        }

        var filePath = usedDb.Avatar!.Path;

        var result = await userRepository.DeleteUserAsync(command.Id, cancellationToken);

        if (result && !string.IsNullOrEmpty(filePath))
        {
            var deleteBlobResult = await blobStorage.DeleteAsync(filePath, cancellationToken);

            if (deleteBlobResult)
            {
                await fileRepository.DeleteAsync(usedDb.Avatar.Id, cancellationToken);
            }
        }
    }
}