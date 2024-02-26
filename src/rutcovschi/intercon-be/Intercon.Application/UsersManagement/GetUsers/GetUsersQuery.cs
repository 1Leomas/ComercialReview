using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.GetUsers;

public sealed record GetUsersQuery() : IQuery<IEnumerable<UserDetailsDto>>;

public sealed class GetUsersQueryHandler(InterconDbContext context) : IQueryHandler<GetUsersQuery, IEnumerable<UserDetailsDto>>
{
    private readonly InterconDbContext _context = context;
    
    public async Task<IEnumerable<UserDetailsDto>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var usersDb = await _context.AspNetUsers.Include(x => x.Avatar).OrderBy(x => x.Id).ToListAsync(cancellationToken);

        var users = new List<UserDetailsDto>();

        foreach (var userDb in usersDb)
        {
            users.Add(userDb.ToUserDetailsDto());
        }

        return users;
    }
}
