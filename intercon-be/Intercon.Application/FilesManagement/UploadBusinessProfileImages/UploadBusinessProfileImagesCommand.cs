using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.FilesManagement.UploadBusinessProfileImages;

public sealed record UploadBusinessProfileImagesCommand(IEnumerable<IFormFile> ProfileImages, int BusinessId) : ICommand<List<BusinessGalleryPhotoDto>>;

internal sealed class UploadBusinessProfileImagesCommandHandler(
    IFileRepository fileRepository,
    IImageValidator imageValidator) : ICommandHandler<UploadBusinessProfileImagesCommand, List<BusinessGalleryPhotoDto>>
{
    public async Task<List<BusinessGalleryPhotoDto>> Handle(UploadBusinessProfileImagesCommand request, CancellationToken cancellationToken)
    {
        var galleryPhotoDtos = new List<BusinessGalleryPhotoDto>();

        foreach (var image in request.ProfileImages)
        {
            if (image.Length == 0) continue;

            var imageData = new ImageData(image.FileName, await image.GetBytesAsync());

            if (!imageValidator.IsValidImage(imageData)) continue;

            var fileDataBd = await fileRepository.UploadFileAsync(image, request.BusinessId, cancellationToken);

            if (fileDataBd is not null)
            {
                galleryPhotoDtos.Add( new BusinessGalleryPhotoDto
                {
                    Id = fileDataBd.Id,
                    Path = fileDataBd.Path
                });
            }
        }

        return galleryPhotoDtos;
    }
}