using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(int BusinessId, EditBusinessDto Data) : ICommand<BusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler(IBusinessRepository businessRepository) : ICommandHandler<EditBusinessCommand, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        var businessDb = await businessRepository.UpdateBusinessAsync(
            command.BusinessId,
            command.Data, 
            cancellationToken);

        if (businessDb is null)
        {
            return null;
        }

        return new BusinessDetailsDto(
            Id: businessDb.Id,
            OwnerId: businessDb.OwnerId,
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