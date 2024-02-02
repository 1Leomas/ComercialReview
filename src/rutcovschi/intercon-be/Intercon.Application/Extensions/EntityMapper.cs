﻿using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain;

namespace Intercon.Application.Extensions;

public static class EntityMapper
{
    public static User ToEntity(this UserDto user)
    {
        if (user != null)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName,
            };
        }

        return null;
    }

    public static UserDto ToDto(this User user)
    {
        if (user != null)
        {
            return new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName,
            };
        }

        return null;
    }

    public static User ToEntity(this CreateUserDto user)
    {
        if (user != null)
        {
            return new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                UserName = user.UserName,
            };
        }

        return null;
    }
}