using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.UserNameUniqueCheck;

public sealed record UserNameUniqueCheckQuery(string UserName) : IQuery<bool>;

public sealed class VerifyUserNameQueryHandler
    (IUserRepository userRepository) : IQueryHandler<UserNameUniqueCheckQuery, bool>
{
    public async Task<bool> Handle(UserNameUniqueCheckQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.UserNameIsUniqueAsync(request.UserName, cancellationToken);
    }
}