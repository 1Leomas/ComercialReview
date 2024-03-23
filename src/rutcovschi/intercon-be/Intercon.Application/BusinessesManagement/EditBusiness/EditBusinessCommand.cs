using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(int BusinessId, EditBusinessDto Data) : ICommand<BusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler
    (IBusinessRepository businessRepository) : ICommandHandler<EditBusinessCommand, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        var businessDb = await businessRepository.UpdateBusinessAsync(
            command.BusinessId,
            command.Data,
            cancellationToken);

        if (businessDb is null) return null;

        return new BusinessDetailsDto(
            businessDb.Id,
            businessDb.OwnerId,
            businessDb.Title,
            businessDb.ShortDescription,
            businessDb.FullDescription,
            businessDb.Rating,
            businessDb.LogoId.HasValue ? new ImageDto(businessDb.Logo!.Data) : null,
            businessDb.Address,
            businessDb.ReviewsCount,
            businessDb.Category
        );
    }
}