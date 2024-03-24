using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.FilesManagement.DeleteFile;
using Intercon.Application.FilesManagement.UploadFile;
using MediatR;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand<UserDetailsDto>;

public sealed record EditUser
{
    public int UserId { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? FirstName  { get; init; }
    public string? LastName  { get; init; }
    public int? AvatarId  { get; init; }
}

public sealed class EditUserCommandHandler(
        IUserRepository userRepository,
        IMediator mediator)
    : ICommandHandler<EditUserCommand, UserDetailsDto>
{
    public async Task<UserDetailsDto> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        FileDataDto? avatarFileData = null!;
        if (command.Data.Avatar is not null)
        {
            avatarFileData = await mediator.Send(new UploadFileCommand(command.Data.Avatar), cancellationToken);

            if (avatarFileData is null) throw new InvalidOperationException("Can not upload avatar");

            var oldAvatarId = await userRepository.GetAvatarIdIfExistsAsync(command.UserId, cancellationToken);

            if (oldAvatarId is not null)
            {
                await mediator.Send(new DeleteFileQuery(oldAvatarId.Value), cancellationToken);
            }
        }

        var userToEdit = new EditUser
        {
            UserId = command.UserId,
            UserName = command.Data.UserName,
            Email = command.Data.Email,
            FirstName = command.Data.FirstName,
            LastName = command.Data.LastName,
            AvatarId = avatarFileData?.Id,
        };

        var userDb = await userRepository.UpdateUserAsync(
            userToEdit,
            cancellationToken);

        if (userDb is null) throw new InvalidOperationException("Can not update user");

        return new UserDetailsDto(
            Id: userDb.Id,
            UserName: userDb.UserName,
            Email: userDb.Email!,
            FirstName: userDb.FirstName,
            LastName: userDb.LastName,
            AvatarPath: userDb.Avatar?.Path 
        );
    }
}