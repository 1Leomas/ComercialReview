using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand<EditUserDto>;

public sealed class EditUserCommandHandler(
        IUserRepository userRepository)
    : ICommandHandler<EditUserCommand, EditUserDto>
{
    public async Task<EditUserDto> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await userRepository.UpdateUserAsync(
            command.UserId,
            command.Data,
            cancellationToken);

        if (userDb is null) throw new InvalidOperationException("Can not update user");

        return new EditUserDto
        {
            UserName = userDb.UserName,
            Email = userDb.Email,
            FirstName = userDb.FirstName,
            LastName = userDb.LastName,
        };
    }
}