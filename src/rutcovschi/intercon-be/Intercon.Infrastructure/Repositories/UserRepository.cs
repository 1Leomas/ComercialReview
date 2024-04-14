using Intercon.Application.Abstractions;
using Intercon.Application.UsersManagement.EditUser;
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
            .AsNoTracking()
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

    public async Task<bool> CreateUserAsync(User newUser, string password, CancellationToken cancellationToken)
    {
        var result = await userManager.CreateAsync(
            new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = string.IsNullOrEmpty(newUser.UserName) ? newUser.Email : newUser.UserName,
                Role = newUser.Role
            },
            password
        );

        return result.Succeeded;
    }

    public async Task<User?> UpdateUserAsync(EditUser newUserData, CancellationToken cancellationToken)
    {
        var userDb = await userManager.Users
            .Include(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == newUserData.UserId, cancellationToken);

        if (userDb == null)
        {
            return null;
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
        if (newUserData.AvatarId.HasValue)
        {
            userDb.AvatarId = newUserData.AvatarId.Value;
        }

        userDb.UpdateDate = DateTime.Now;
        userDb.WasEdited = true;

        await context.SaveChangesAsync(cancellationToken);

        return userDb;
    }

    public async Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var userDb = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (userDb == null)
        {
            return false;
        }

        var rows = await userManager.Users.Where(x => x.Id == userDb.Id).ExecuteDeleteAsync(cancellationToken);

        return rows != 0;
    }

    public async Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await userManager.Users.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken)
    {
        return await userManager.FindByNameAsync(userName) == null;
    }

    public async Task<bool> UserEmailExistsAsync(string userEmail, CancellationToken cancellationToken)
    {
        return await userManager.FindByEmailAsync(userEmail) != null;
    }

    public async Task<bool> NewUserEmailIsFreeAsync(int userId, string userEmail, CancellationToken cancellationToken)
    {
        return !(await context.Users.AnyAsync(x => x.Email == userEmail && x.Id != userId, cancellationToken));
    }

    public async Task<bool> NewUsernameIsFreeAsync(int userId, string username, CancellationToken cancellationToken)
    {
        return !(await context.Users.AnyAsync(x => x.UserName == username && x.Id != userId, cancellationToken));
    }

    public async Task<int> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users.Where(x => x.Email == email).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int?> GetAvatarIdIfExistsAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Users
            .Where(x => x.Id == userId)
            .Select(x => x.AvatarId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}