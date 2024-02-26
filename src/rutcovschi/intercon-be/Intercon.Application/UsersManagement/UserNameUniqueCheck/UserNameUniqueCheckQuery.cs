using Intercon.Application.Abstractions.Messaging;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.VerifyUserName;

public sealed record UserNameUniqueCheckQuery(string UserName) : IQuery<bool>;

public sealed class VerifyUserNameQueryHandler(InterconDbContext context) : IQueryHandler<UserNameUniqueCheckQuery, bool>
{
    private readonly InterconDbContext _context = context;

    public async Task<bool> Handle(UserNameUniqueCheckQuery request, CancellationToken cancellationToken)
    {
        if (request.UserName.IsNullOrEmpty()) 
        { 
            return false; 
        }

        return await _context.AspNetUsers.AllAsync(x => x.UserName != request.UserName);
    }
}