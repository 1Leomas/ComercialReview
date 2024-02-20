using FluentValidation;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public record EditBusinessDto(
    string Title,
    string ShortDescription);

public sealed record EditBusinessCommand(int BusinessId, EditBusinessDto Data) : ICommand<BusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler(InterconDbContext context) : ICommandHandler<EditBusinessCommand, BusinessDetailsDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<BusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        var businessDb = await _context.Businesses
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(b => b.Id == command.BusinessId, cancellationToken);

        if(businessDb is null)
        {
            return null;
        }

        if(!command.Data.Title.IsNullOrEmpty())
        {
            businessDb.Title = command.Data.Title;
        }
        if (!command.Data.ShortDescription.IsNullOrEmpty())
        {
            businessDb.ShortDescription = command.Data.ShortDescription;
        }

        //de adaugat si restu

        await _context.SaveChangesAsync(cancellationToken);

        return new BusinessDetailsDto(
            Id: businessDb.Id,
            Title: businessDb.Title,
            ShortDescription: businessDb.ShortDescription,
            FullDescription: businessDb.FullDescription,
            Rating: businessDb.Rating,
            Logo: businessDb.LogoId.HasValue ? new ImageDto(Data: businessDb.Logo!.Data) : null,
            Address: businessDb.Address,
            ReviewsCount: businessDb.ReviewsCount,
            Category: businessDb.Category
        );
    }
}

public class EditBusinessCommandValidator : AbstractValidator<EditBusinessCommand>
{
    public EditBusinessCommandValidator(InterconDbContext dbContext)
    {
        RuleFor(x => x.BusinessId).NotEmpty();

        RuleFor(x => x.BusinessId)
            .MustAsync(async (businessId, ctx) =>
            {
                return await dbContext.Businesses.AnyAsync(x => x.Id == businessId, ctx);
            });

        When(x => !string.IsNullOrEmpty(x.Data.Title), () =>
        {
            RuleFor(x => x.Data.Title)
            .MaximumLength(255)
            .WithName(x => nameof(x.Data.Title));
        });
        When(x => !string.IsNullOrEmpty(x.Data.ShortDescription), () =>
        {
            RuleFor(x => x.Data.ShortDescription)
            .MaximumLength(500)
            .WithName(x => nameof(x.Data.ShortDescription));
        });
    }
}