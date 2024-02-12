using FluentValidation;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public class CreateBusinessCommandValidator : AbstractValidator<CreateBusinessCommand>
{
    public CreateBusinessCommandValidator(InterconDbContext context)
    {
        RuleFor(x => x.Data.OwnerId)
            .NotEmpty()
            .MustAsync(async (ownerId, ctx) => await context.Businesses
                .AllAsync(x => x.OwnerId != ownerId))
            .WithMessage("The user already owns a business");

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