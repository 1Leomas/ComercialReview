using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.FilesManagement.DeleteFile;
using Intercon.Application.FilesManagement.UploadFile;
using MediatR;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(int CurrentUserId, int BusinessId, EditBusinessDto Data) : ICommand<EditBusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler(
    IBusinessRepository businessRepository,
    IMediator mediator) : ICommandHandler<EditBusinessCommand, EditBusinessDetailsDto?>
{
    public async Task<EditBusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        FileDataDto? logoFileData = null!;
        if (command.Data.Logo is not null)
        {
            logoFileData = await mediator.Send(new UploadFileCommand(command.Data.Logo), cancellationToken);

            var businessOldLogoId = await businessRepository.GetBusinessLogoIdAsync(command.BusinessId, cancellationToken);

            if (businessOldLogoId is not null)
                await mediator.Send(new DeleteFileQuery(businessOldLogoId.Value), cancellationToken);
        }

        var businessDb = await businessRepository.UpdateBusinessAsync(
            command.BusinessId,
            command.Data,
            logoFileData?.Id,
            cancellationToken);

        if (businessDb is null) return null;

        return new EditBusinessDetailsDto
        {
            Title = businessDb.Title,
            ShortDescription = businessDb.ShortDescription,
            FullDescription = businessDb.FullDescription,
            Category = businessDb.Category,
            Address = new AddressDto
            {
                Latitude = businessDb.Address.Latitude,
                Longitude = businessDb.Address.Longitude,
                Street = businessDb.Address.Street,
            },
            LogoPath = businessDb.Logo?.Path,
        };
    }
}