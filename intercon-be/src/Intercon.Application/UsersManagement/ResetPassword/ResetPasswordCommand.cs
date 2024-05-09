using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Services;
using Intercon.Application.DataTransferObjects.User;
using Intercon.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Intercon.Application.UsersManagement.ResetPassword;

public sealed record ResetPasswordCommand(ResetPasswordRequest ResetPasswordRequest) : ICommand;

internal sealed class ResetPasswordCommandHandler(UserManager<User> userManager, IIdentityService identityService, IEmailService emailService) : ICommandHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.ResetPasswordRequest.Email);

        if (user is null)
        {
            return;
        }

        var result = await identityService.VerifyResetPasswordCode(user.Id, request.ResetPasswordRequest.ResetPasswordCode);

        if (!result)
        {
            throw new DataException("Invalid or expired reset password code");
        }

        var identityPasswordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        await userManager.ResetPasswordAsync(user, identityPasswordResetToken, request.ResetPasswordRequest.Password);

        await emailService.SendEmailAsync(
            user.Email,
$"{user.FirstName} {user.LastName}",
            "Reset password success",
            "Your password was successfully reseated");
    }
}

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator(UserManager<User> userManager)
    {
        RuleFor(x => x.ResetPasswordRequest.Email)
            .NotEmpty()
            .EmailAddress()
            .DependentRules(() =>
            {
                RuleFor(x => x.ResetPasswordRequest.Email)
                    .MustAsync(async (email, _) => await userManager.FindByEmailAsync(email) is not null)
                    .WithMessage("The email does not exist");
            })
            .WithName(x => nameof(x.ResetPasswordRequest.Email));

        RuleFor(x => x.ResetPasswordRequest.Password)
            .NotEmpty()
            .Length(8, 50)
            .WithName(x => nameof(x.ResetPasswordRequest.Password));

        RuleFor(x => x.ResetPasswordRequest.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.ResetPasswordRequest.Password)
            .WithName(x => nameof(x.ResetPasswordRequest.ConfirmPassword))
            .WithMessage("Passwords do not match");
    }
}