using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.FilesManagement.DeleteFile;
using MediatR;

namespace Intercon.Application.UsersManagement.SetUserAvatarId;

public sealed record SetUserAvatarIdCommand(int UserId, int FileId) : ICommand;

internal sealed class SetUserAvatarIdCommandHandler(
    IMediator mediator,
    IUserRepository userRepository) : ICommandHandler<SetUserAvatarIdCommand>
{
    public async Task Handle(SetUserAvatarIdCommand command, CancellationToken cancellationToken)
    {
        var userAvatarId = await userRepository.GetAvatarIdIfExistsAsync(command.UserId, cancellationToken);

        if (userAvatarId is not null && userAvatarId != 0)
        {
            await mediator.Send(new DeleteFileQuery(userAvatarId!.Value), cancellationToken);
        }

        await userRepository.UpdateUserAvatarAsync(command.UserId, command.FileId, cancellationToken);
    }
}