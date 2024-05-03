using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.BusinessesManagement.DeleteBusinessGalleryPhoto;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.Exceptions;
using Intercon.Application.FilesManagement.DeleteFile;
using Intercon.Application.FilesManagement.UploadBusinessGalleryPhotos;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Domain.Entities;
using MediatR;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(
        int CurrentUserId, 
        int BusinessId, 
        EditBusinessRequest Data, 
        int? NewLogoId) 
    : ICommand<BusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler(
    IBusinessRepository businessRepository,
    IMediator mediator) : ICommandHandler<EditBusinessCommand, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        if (command.NewLogoId is not null)
        {
            var businessOldLogoId = 
                await businessRepository.GetBusinessLogoIdAsync(command.BusinessId, cancellationToken);

            if (businessOldLogoId is not null)
                await mediator.Send(new DeleteFileCommand(businessOldLogoId.Value), cancellationToken);
        }

        if (command.Data.PhotosToDelete is not null && command.Data.PhotosToDelete.Length != 0)
        {
            foreach (var photoId in command.Data.PhotosToDelete)
            {
                await mediator.Send(new DeleteBusinessGalleryPhotoCommand(command.BusinessId, command.CurrentUserId, photoId), cancellationToken);
            }
        }

        if (command.Data.GalleryPhotos is not null && command.Data.GalleryPhotos.Any())
        {
            await mediator.Send(new UploadBusinessGalleryPhotosCommand(
                    command.Data.GalleryPhotos, command.BusinessId),
                cancellationToken);
        }

        var business = await businessRepository.UpdateBusinessAsync(
            command.BusinessId,
            command.Data,
            command.NewLogoId,
            cancellationToken);

        if (business is null) return null;

        return new BusinessDetailsDto(
            business.Id,
            business.OwnerId,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.LogoId is not null ? business.Logo?.Path : null,
            business.GalleryPhotos.Select(photo => new BusinessGalleryPhotoDto { Id = photo.Id, Path = photo.Path }),
            business.Address,
            business.ReviewsCount,
            (int)business.Category);
    }
}