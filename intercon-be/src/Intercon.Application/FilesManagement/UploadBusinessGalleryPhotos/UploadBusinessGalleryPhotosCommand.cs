using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.FilesManagement.UploadBusinessGalleryPhotos;

public sealed record UploadBusinessGalleryPhotosCommand(IEnumerable<IFormFile> GalleryPhotos, int BusinessId) : ICommand<List<BusinessGalleryPhotoDto>>;

internal sealed class UploadBusinessGalleryPhotosCommandHandler(
    IBlobStorage blobStorage,
    IFileRepository fileRepository,
    IImageValidator imageValidator,
    ILogger<UploadBusinessGalleryPhotosCommandHandler> logger) : ICommandHandler<UploadBusinessGalleryPhotosCommand, List<BusinessGalleryPhotoDto>>
{
    public async Task<List<BusinessGalleryPhotoDto>> Handle(UploadBusinessGalleryPhotosCommand request, CancellationToken cancellationToken)
    {
        var galleryPhotos = new List<BusinessGalleryPhotoDto>();

        foreach (var image in request.GalleryPhotos)
        {
            if (image.Length == 0) continue;

            var imageData = new ImageData(image.FileName, await image.GetBytesAsync());

            if (!imageValidator.IsValidImage(imageData))
            {
                logger.LogWarning($"Invalid image format: {imageData.ContentType}");
                continue;
            }

            var blob = await blobStorage.UploadAsync(image, cancellationToken);

            var fileDataBd = await fileRepository.UploadFileAsync(blob, request.BusinessId, cancellationToken);

            galleryPhotos.Add(new BusinessGalleryPhotoDto
            {
                Id = fileDataBd.Id,
                Path = fileDataBd.Path
            });
        }

        return galleryPhotos;
    }
}