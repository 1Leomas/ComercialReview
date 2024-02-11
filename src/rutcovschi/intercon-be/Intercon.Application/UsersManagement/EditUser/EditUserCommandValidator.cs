﻿using FluentValidation;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.UsersManagement.EditUser;

public sealed class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator(InterconDbContext dbContext)
    {
        When(x => !string.IsNullOrEmpty(x.Data.FirstName), () =>
        {
            RuleFor(x => x.Data.FirstName)
            .MaximumLength(50)
            .WithName(x => nameof(x.Data.FirstName));
        });
        When(x => !string.IsNullOrEmpty(x.Data.LastName), () =>
        {
            RuleFor(x => x.Data.LastName)
            .MaximumLength(50)
            .WithName(x => nameof(x.Data.LastName));
        });

        When(x => !string.IsNullOrEmpty(x.Data.Email), () =>
        {
            RuleFor(x => x.Data.Email)
                .EmailAddress()
                .MustAsync(async (email, ctx) => await dbContext.Users.AllAsync(x => x.Email != email, ctx))
                .WithMessage("The email must be unique")
                .WithName(x => nameof(x.Data.Email));
        });

        When(x => !string.IsNullOrEmpty(x.Data.UserName), () =>
        {
            RuleFor(x => x.Data.UserName)
            .Length(2, 50)
            .WithName(x => nameof(x.Data.UserName));

            RuleFor(x => x).MustAsync(async (command, ctx) => 
                await dbContext.Users.AllAsync(
                    x => x.Id == command.UserId || x.UserName != command.Data.UserName, ctx)
            ).WithName(x => nameof(x.Data.UserName)).WithMessage("The username must be unique");
        });
    }
}