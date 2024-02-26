using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.UsersManagement.VerifyUserName;

public sealed record UserNameUniqueCheckQuery(string UserName) : IQuery<bool>;

public sealed class VerifyUserNameQueryHandler(UserManager<User> userManager) : IQueryHandler<UserNameUniqueCheckQuery, bool>
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task<bool> Handle(UserNameUniqueCheckQuery request, CancellationToken cancellationToken)
    {
        if (request.UserName.IsNullOrEmpty()) 
        { 
            return false; 
        }

        return await _userManager.FindByNameAsync(request.UserName) is null;
    }
}