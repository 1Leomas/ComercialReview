using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class UserMapper
{
    public static User ToEntity(this UserDto user)
    {
        return new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            //Password = user.Password,
            UserName = user.UserName
        };
    }

    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            //Password = user.Password,
            UserName = user.UserName
        };
    }

    public static User ToEntity(this RegisterUserDto user)
    {
        return new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            //Password = user.Password,
            UserName = user.UserName
        };
    }

    public static UserDetailsDto ToUserDetailsDto(this User user)
    {
        return new UserDetailsDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.UserName,
            user.Avatar?.Path
        );
    }
}