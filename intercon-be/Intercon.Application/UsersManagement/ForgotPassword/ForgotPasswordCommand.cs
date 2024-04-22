using System.Text.Encodings.Web;
using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Intercon.Application.UsersManagement.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : ICommand;

internal sealed class ForgotPasswordCommandHandler(
    UserManager<User> userManager,
    IIdentityService identityService,
    IEmailService emailService) : ICommandHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
        {
            return;
        }

        var resetPasswordCode = await identityService.GenerateResetPasswordCodeAsync(user.Id);

        await emailService.SendEmailAsync(
            user.Email, 
            $"{user.FirstName} {user.LastName}", 
            "Reset Password",
            $"Your reset password code: {resetPasswordCode}. Use it on reset password page to reset your password.");
    }
}
