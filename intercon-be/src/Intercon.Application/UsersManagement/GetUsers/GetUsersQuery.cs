using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.UsersManagement.GetUsers;

public sealed record GetUsersQuery : IQuery<IEnumerable<UserDetailsDto>>;

public sealed class GetUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersQuery, IEnumerable<UserDetailsDto>>
{
    public async Task<IEnumerable<UserDetailsDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var usersDb = await userRepository.GetAllUsersAsync(cancellationToken);

        return usersDb.Select(userDb => userDb.ToUserDetailsDto()).ToList();
    }
}