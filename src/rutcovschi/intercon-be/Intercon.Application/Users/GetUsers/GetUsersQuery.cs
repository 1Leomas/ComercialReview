using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.GetUsers;

public sealed record GetUsersQuery() : IQuery<IEnumerable<UserDto>>;

public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly InterconDbContext _context;

    public GetUsersQueryHandler(InterconDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var usersDb = await _context.Users.OrderBy(x => x.Id).ToListAsync(cancellationToken);

        var users = new List<UserDto>();

        usersDb.ForEach(userDb => users.Add(userDb.ToDto()));

        return users;
    }
}
