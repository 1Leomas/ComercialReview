﻿using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class UserRepository(
    InterconDbContext context,
    UserManager<User> userManager) 
    : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await userManager.Users
        .Include(x => x.Avatar)
        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var usersDb = await userManager.Users
            .Include(x => x.Avatar)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken); //this operation is not efficient, but for now it's ok

        return usersDb;
    }

    public async Task<bool> CreateUserAsync(User newUser, string password, int? avatarId, CancellationToken cancellationToken)
    {
        var result = await userManager.CreateAsync(
            new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = string.IsNullOrEmpty(newUser.UserName) ? newUser.Email : newUser.UserName,
                Role = newUser.Role,
                AvatarId = avatarId
            },
            password
        );

        return result.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(User newUserData, CancellationToken cancellationToken)
    {
        var userDb = await userManager.Users
            .FirstOrDefaultAsync(x => x.Id == newUserData.Id, cancellationToken);

        if (userDb == null)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(newUserData.FirstName))
        {
            userDb.FirstName = newUserData.FirstName;
        }
        if (!string.IsNullOrEmpty(newUserData.LastName))
        {
            userDb.LastName = newUserData.LastName;
        }
        if (!string.IsNullOrEmpty(newUserData.Email))
        {
            userDb.Email = newUserData.Email;
        }
        if (!string.IsNullOrEmpty(newUserData.UserName))
        {
            userDb.UserName = newUserData.UserName;
        }

        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (userDb == null)
        {
            return;
        }

        await context.Images.Where(x => x.Id == userDb.AvatarId).ExecuteDeleteAsync(cancellationToken);
        await userManager.Users.Where(x => x.Id == userDb.Id).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await userManager.Users.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken)
    {
        return await userManager.FindByNameAsync(userName) != null;
    }

    public async Task<bool> UserEmailExistsAsync(string userEmail, CancellationToken cancellationToken)
    {
        return await userManager.FindByEmailAsync(userEmail) != null;
    }
}