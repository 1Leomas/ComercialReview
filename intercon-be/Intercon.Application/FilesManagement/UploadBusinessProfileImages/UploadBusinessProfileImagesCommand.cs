using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.FilesManagement.UploadBusinessProfileImages;

public sealed record UploadBusinessProfileImagesCommand(IEnumerable<IFormFile> ProfileImages, int BusinessId) : ICommand<List<string>>;

internal sealed class UploadBusinessProfileImagesCommandHandler(
    IFileRepository fileRepository,
    IImageValidator imageValidator) : ICommandHandler<UploadBusinessProfileImagesCommand, List<string>>
{
    public async Task<List<string>> Handle(UploadBusinessProfileImagesCommand request, CancellationToken cancellationToken)
    {
        var profileImages = new List<string>();

        foreach (var image in request.ProfileImages)
        {
            if (image.Length == 0) continue;

            var imageData = new ImageData(image.FileName, await image.GetBytesAsync());

            if (!imageValidator.IsValidImage(imageData)) continue;

            var fileDataBd = await fileRepository.UploadFileAsync(image, request.BusinessId, cancellationToken);

            if (fileDataBd is not null)
            {
                profileImages.Add(fileDataBd.Path);
            }
        }

        return profileImages;
    }
}