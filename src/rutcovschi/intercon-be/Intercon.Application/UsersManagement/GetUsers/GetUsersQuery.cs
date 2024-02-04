using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.GetUsers;

public sealed record GetUsersQuery() : IQuery<IEnumerable<UserDetailsDto>>;

public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserDetailsDto>>
{
    private readonly InterconDbContext _context;

    public GetUsersQueryHandler(InterconDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<UserDetailsDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var usersDb = await _context.Users.OrderBy(x => x.Id).ToListAsync(cancellationToken);

        var users = new List<UserDetailsDto>();

        foreach (var userDb in usersDb)
        {
            users.Add(new UserDetailsDto()
            {
                Id = userDb.Id,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
                UserName = userDb.UserName,
            });
        }

        return users;
    }
}
