using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<bool> CreateUserAsync(User newUser, string password, int? avatarId, CancellationToken cancellationToken);
    Task<bool> UpdateUserAsync(User newUserData, CancellationToken cancellationToken);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> UserNameIsUniqueAsync(string userName, CancellationToken cancellationToken);
    Task<bool> UserEmailExistsAsync(string userEmail, CancellationToken cancellationToken);
}