using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.Users.GetUser;

public sealed record GetUserQuery(int Id) : IQuery<UserDto?>;

internal sealed class GetUserQueryHandler(InterconDbContext context) : IQueryHandler<GetUserQuery, UserDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<UserDto?> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Id == query.Id,
            cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
