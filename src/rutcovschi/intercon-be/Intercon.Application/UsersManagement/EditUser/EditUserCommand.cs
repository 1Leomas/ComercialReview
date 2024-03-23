using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand<UserDetailsDto>;

public sealed class EditUserCommandHandler(
        IUserRepository userRepository)
    : ICommandHandler<EditUserCommand, UserDetailsDto>
{
    public async Task<UserDetailsDto> Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        var userDb = await userRepository.UpdateUserAsync(
            command.UserId,
            command.Data,
            cancellationToken);

        if (userDb is null) throw new InvalidOperationException("Can not update user");

        return userDb.ToUserDetailsDto();
    }
}