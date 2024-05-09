using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.Abstractions.Services;
using Intercon.Application.Exceptions;

namespace Intercon.Application.BusinessesManagement.DeleteBusinessGalleryPhoto;

public sealed record DeleteBusinessGalleryPhotoCommand(int BusinessId, int CurrentUserId, int PhotoId) : ICommand;

internal sealed class DeleteBusinessGalleryPhotoCommandHandler(
    IBusinessRepository businessRepository,
    IBlobStorage blobStorage,
    IFileRepository fileRepository) : ICommandHandler<DeleteBusinessGalleryPhotoCommand>
{
    public async Task Handle(DeleteBusinessGalleryPhotoCommand request, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetBusinessByIdAsync(request.BusinessId, cancellationToken);

        if (business is null)
        {
            throw new NotFoundException("Business");
        }

        if (business.OwnerId != request.CurrentUserId)
        {
            throw new ArgumentException("You are not the owner of this business");
        }

        var photo = business.GalleryPhotos.FirstOrDefault(x => x.Id == request.PhotoId);

        if (photo is null)
        {
            throw new NotFoundException("Photo not found");
        }

        var deleteBlobResult = await blobStorage.DeleteAsync(photo.Path, cancellationToken);

        if (!deleteBlobResult)
        {
            throw new InvalidOperationException("Cannot delete file from Blob Storage");
        }

        var deleteFromBdResult = await fileRepository.DeleteAsync(photo.Id, cancellationToken);

        if (!deleteFromBdResult)
        {
            throw new InvalidOperationException("Cannot delete file from database");
        }
    }
}
