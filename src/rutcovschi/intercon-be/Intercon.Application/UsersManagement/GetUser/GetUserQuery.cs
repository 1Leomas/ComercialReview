using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.GetUser;

public sealed record GetUserQuery(int Id) : IQuery<UserDetailsDto?>;

public sealed class GetUserQueryHandler(UserManager<User> userManager) : IQueryHandler<GetUserQuery, UserDetailsDto?>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<UserDetailsDto?> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(x => x.Avatar)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        return user?.ToUserDetailsDto();
    }
}
