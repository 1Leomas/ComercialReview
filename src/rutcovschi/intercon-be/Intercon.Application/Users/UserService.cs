using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain;
using Intercon.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users;

public class UserService
{
    private readonly InterconDbContext _context;

    public UserService(InterconDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null) 
        {
            return null;
        }

        return new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var usersDb = await _context.Users.OrderBy(x => x.Id).ToListAsync();

        var users = new List<UserDto>();

        foreach (var userDb in usersDb)
        {
            var user = new UserDto()
            {
                Id = userDb.Id,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email
            };
            users.Add(user);
        }

        return users;
    }

    public async Task<int> CreateUser(UserDto userToAdd)
    {
        var userDb = new User()
        {
            FirstName = userToAdd.FirstName,
            LastName = userToAdd.LastName,
            Email = userToAdd.Email
        };

        await _context.Users.AddAsync(userDb);
        await _context.SaveChangesAsync();

        return userDb.Id;
    }

    public async Task<Unit?> DeleteUser(int id)
    {
        var noteTodelete = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (noteTodelete == null)
        {
            return null;
        }

        _context.Users.Remove(noteTodelete);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
