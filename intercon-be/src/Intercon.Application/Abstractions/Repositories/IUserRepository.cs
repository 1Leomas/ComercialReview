using Intercon.Application.UsersManagement.EditUser;
using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<bool> CreateUserAsync(User newUser, string password, CancellationToken cancellationToken);
    Task<User?> UpdateUserAsync(EditUser newUserData, CancellationToken cancellationToken);
    Task<bool> DeleteUserAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken);
    Task<bool> UserEmailExistsAsync(string userEmail, CancellationToken cancellationToken);
    Task<bool> NewUserEmailIsFreeAsync(int userId, string userEmail, CancellationToken cancellationToken);
    Task<bool> NewUsernameIsFreeAsync(int userId, string userName, CancellationToken cancellationToken);
    Task<int> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken);
    Task<int?> GetAvatarIdIfExistsAsync(int userId, CancellationToken cancellationToken);
}