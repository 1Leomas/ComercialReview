using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(int CurrentUserId, int BusinessId, EditBusinessDto Data) : ICommand<EditBusinessDto?>;

public sealed class EditBusinessCommandHandler
    (IBusinessRepository businessRepository) : ICommandHandler<EditBusinessCommand, EditBusinessDto?>
{
    public async Task<EditBusinessDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        var businessDb = await businessRepository.UpdateBusinessAsync(
            command.BusinessId,
            command.Data,
            cancellationToken);

        if (businessDb is null) return null;

        if (businessDb.OwnerId != command.CurrentUserId)
            throw new InvalidOperationException("Current user is not the owner of this business");

        return new EditBusinessDto(
            businessDb.Title,
            businessDb.ShortDescription,
            businessDb.FullDescription,
            businessDb.Category,
            new AddressDto
            {
                Latitude = businessDb.Address.Latitude,
                Longitude = businessDb.Address.Longitude,
                Street = businessDb.Address.Street
            }
        );
    }
}