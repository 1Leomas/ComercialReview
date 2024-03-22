using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(int UserId, CreateBusinessDto Data) : ICommand<BusinessDetailsDto>;

public sealed class CreateBusinessCommandHandler(
    IImageRepository imageRepository,
    IBusinessRepository businessRepository,
    ILogger<CreateBusinessCommandHandler> logger) 
    : ICommandHandler<CreateBusinessCommand, BusinessDetailsDto>
{
    public async Task<BusinessDetailsDto> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        int? logoId = null!;

        if (command.Data.Logo is not null)
        {
            logoId = await imageRepository.AddImage(
                new Image { Data = command.Data.Logo.Data },
                cancellationToken);

            if (!logoId.HasValue)
            {
                logger.LogError("Can not add logo to business");
            }
        }

        var businessDb = new Business(){

            OwnerId = command.UserId,
            Title = command.Data.Title,
            ShortDescription = command.Data.ShortDescription,
            FullDescription = command.Data.FullDescription,
            Address = command.Data.Address,
            Category = command.Data.Category,
            LogoId = logoId
        };

        var businessId = await businessRepository.CreateBusinessAsync(businessDb, cancellationToken);

        return new BusinessDetailsDto(
            Id: businessId,
            OwnerId: command.UserId,
            Title: businessDb.Title,
            ShortDescription: businessDb.ShortDescription,
            FullDescription: businessDb.FullDescription,
            Rating: businessDb.Rating,
            Logo: businessDb.LogoId.HasValue ? new ImageDto(Data: command.Data.Logo!.Data) : null,
            Address: businessDb.Address,
            ReviewsCount: businessDb.ReviewsCount,
            Category: businessDb.Category
        );
    }
}