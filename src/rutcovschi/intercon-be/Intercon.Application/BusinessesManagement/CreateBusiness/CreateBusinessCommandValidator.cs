using FluentValidation;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator(InterconDbContext context, UserManager<User> userManager)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(async (ownerId, ctx) => await context.Businesses
                .AllAsync(x => x.OwnerId != ownerId))
            .WithMessage("The user already owns a business")
            .DependentRules(() =>
            {
                RuleFor(x => x.UserId)
                    .MustAsync(async (userId, ctx) => await userManager.Users.AnyAsync(x => x.Id == userId, ctx))
                    .WithMessage("The user doesn't exists");
            }); ;

        RuleFor(x => x.Data.Title)
            .NotEmpty()
            .MaximumLength(255)
            .WithName(x => nameof(x.Data.Title));

        RuleFor(x => x.Data.ShortDescription)
            .NotEmpty()
            .MaximumLength(500)
            .WithName(x => nameof(x.Data.ShortDescription));

        RuleFor(x => x.Data.Address)
            .NotEmpty()
            .WithName(x => nameof(x.Data.Address));
    }
}