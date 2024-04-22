using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.UsersManagement.GetUser;

public sealed record GetUserQuery(int Id) : IQuery<UserDetailsDto?>;

public sealed class GetUserQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserQuery, UserDetailsDto?>
{
    public async Task<UserDetailsDto?> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(query.Id, cancellationToken);

        return user?.ToUserDetailsDto();
    }
}