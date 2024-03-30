using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;

namespace Intercon.Application.UsersManagement.VerifyPasswordResetCode;

public sealed record VerifyPasswordResetCodeQuery(string PasswordResetCode) : IQuery<bool>;

internal sealed class VerifyPasswordResetCodeQueryHandler(IIdentityService identityService) 
    : IQueryHandler<VerifyPasswordResetCodeQuery, bool>
{
    public async Task<bool> Handle(VerifyPasswordResetCodeQuery query, CancellationToken cancellationToken)
    {
        return await identityService.VerifyResetPasswordCode(query.PasswordResetCode);
    }
}