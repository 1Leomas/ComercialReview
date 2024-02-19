using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.GetUser;

public sealed record GetUserQuery(int Id) : IQuery<UserDetailsDto?>;

public sealed class GetUserQueryHandler(InterconDbContext context) : IQueryHandler<GetUserQuery, UserDetailsDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<UserDetailsDto?> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (user == null)
        {
            return null;
        }

        return user.ToUserDetailsDto();
    }
}
