using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using MediatR;

namespace Intercon.Application.UsersManagement.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserDto Data) : ICommand;

public sealed class RegisterUserCommandHandler(
        IMediator mediator,
        IUserRepository userRepository)
    : ICommandHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            FirstName = command.Data.FirstName,
            LastName = command.Data.LastName,
            Email = command.Data.Email,
            UserName = string.IsNullOrEmpty(command.Data.UserName) ? command.Data.Email : command.Data.UserName,
            Role = (Role)command.Data.Role,
        };

        var isSuccess =
            await userRepository.CreateUserAsync(newUser, command.Data.Password, cancellationToken);

        if (!isSuccess) throw new Exception("Can not register user");
    }
}