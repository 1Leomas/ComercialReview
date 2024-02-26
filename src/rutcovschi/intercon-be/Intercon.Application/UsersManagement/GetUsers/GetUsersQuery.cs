using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.GetUsers;

public sealed record GetUsersQuery() : IQuery<IEnumerable<UserDetailsDto>>;

public sealed class GetUsersQueryHandler(UserManager<User> userManager) : IQueryHandler<GetUsersQuery, IEnumerable<UserDetailsDto>>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<IEnumerable<UserDetailsDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var usersDb = await _userManager.Users
            .Include(x => x.Avatar)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken); //this operations is not efficient, but for now it's ok

        return usersDb.Select(userDb => userDb.ToUserDetailsDto()).ToList();
    }
}
