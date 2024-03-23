using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

namespace Intercon.Application.UsersManagement.RegisterUser;

public sealed record RegisterUserCommand(RegisterUserDto Data) : ICommand;

public sealed class RegisterUserCommandHandler(
        IImageRepository imageRepository,
        IUserRepository userRepository)
    : ICommandHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        int? avatarId = null!;

        if (command.Data.Avatar is not null)
            avatarId = await imageRepository.AddImage(
                new Image { Data = command.Data.Avatar.Data },
                cancellationToken);

        var newUser = new User
        {
            FirstName = command.Data.FirstName,
            LastName = command.Data.LastName,
            Email = command.Data.Email,
            UserName = string.IsNullOrEmpty(command.Data.UserName) ? command.Data.Email : command.Data.UserName,
            Role = (Role)command.Data.Role,
            AvatarId = avatarId
        };

        var isSuccess =
            await userRepository.CreateUserAsync(newUser, command.Data.Password, avatarId, cancellationToken);

        if (!isSuccess) throw new Exception("Can not register ApplicationUser");
    }
}