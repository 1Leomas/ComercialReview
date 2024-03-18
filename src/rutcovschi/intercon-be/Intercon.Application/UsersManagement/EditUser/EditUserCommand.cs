using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed record EditUserCommand(int UserId, EditUserDto Data) : ICommand;

public sealed class EditUserCommandHandler(
    IUserRepository userRepository) 
    : ICommandHandler<EditUserCommand>
{
    public async Task Handle(EditUserCommand command, CancellationToken cancellationToken)
    {
        await userRepository.UpdateUserAsync(new User()
        {
            Id = command.UserId,
            FirstName = command.Data.FirstName,
            LastName = command.Data.LastName,
            Email = command.Data.Email,
            UserName = string.IsNullOrEmpty(command.Data.UserName) ? command.Data.Email : command.Data.UserName
        }, cancellationToken);
    }
}