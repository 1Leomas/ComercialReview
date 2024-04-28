using FluentValidation;
using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.FilesManagement.UploadFile;

public sealed record UploadFileCommand(IFormFile ImageData) : ICommand<FileDataDto?>;

internal sealed class UploadFileCommandHandler(
    IBlobStorage blobStorage,
    IFileRepository fileRepository) : ICommandHandler<UploadFileCommand, FileDataDto?>
{
    public async Task<FileDataDto?> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var azureFilePath = await blobStorage.UploadAsync(request.ImageData, cancellationToken);

        var fileDataBd = await fileRepository.UploadFileAsync(azureFilePath, cancellationToken);

        var fileData = new FileDataDto(fileDataBd.Id, fileDataBd.Path);

        return fileData;
    }
}

public sealed class UploadFileValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileValidator(IImageValidator imageValidator)
    {
        RuleFor(x => x.ImageData.Length)
            .GreaterThan(0)
            .WithMessage("No file uploaded");

        RuleFor(x => x.ImageData)
            .MustAsync(async (data, ctx) =>
            {
                var fileData = new ImageData(data.FileName, await data.GetBytesAsync());

                return imageValidator.IsValidImage(fileData);
            }).WithMessage("Bad file type");
    }
}