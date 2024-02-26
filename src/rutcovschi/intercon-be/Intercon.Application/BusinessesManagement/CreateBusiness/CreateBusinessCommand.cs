using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(CreateBusinessDto Data) : ICommand<BusinessDetailsDto>;

public sealed class CreateBusinessCommandHandler(InterconDbContext context) : ICommandHandler<CreateBusinessCommand, BusinessDetailsDto>
{
    private readonly InterconDbContext _context = context;

    public async Task<BusinessDetailsDto> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        var userExits = await _context.AspNetUsers.AnyAsync(x => x.Id == command.Data.OwnerId, cancellationToken);
       
        if (!userExits)
        {
            throw new Exception("User not found");
        }

        var businessDb = command.Data.ToEntity();

        // maybe make a separate request for this
        Image? image = null;
        if (command.Data.Logo != null)
        {
            image = new Image()
            {
                Data = command.Data.Logo.Data
            };

            await _context.Images.AddAsync(image, cancellationToken);
            var rows = await _context.SaveChangesAsync(cancellationToken);

            businessDb.LogoId = rows != 0 ? image.Id : null!;
        }

        await _context.Businesses.AddAsync(businessDb, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new BusinessDetailsDto(
            Id: businessDb.Id,
            Title: businessDb.Title,
            ShortDescription: businessDb.ShortDescription,
            FullDescription: businessDb.FullDescription,
            Rating: businessDb.Rating,
            Logo: businessDb.LogoId.HasValue ? new ImageDto(Data: image!.Data) : null,
            Address: businessDb.Address,
            ReviewsCount: businessDb.ReviewsCount,
            Category: businessDb.Category
        );
    }
}