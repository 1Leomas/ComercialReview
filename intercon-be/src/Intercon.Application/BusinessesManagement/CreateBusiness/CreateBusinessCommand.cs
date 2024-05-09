using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.FilesManagement.UploadBusinessGalleryPhotos;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(int UserId, CreateBusinessDto Data) : ICommand<BusinessDetailsDto>;

public sealed class CreateBusinessCommandHandler(
        IMediator mediator,
        IBusinessRepository businessRepository,
        ILogger<CreateBusinessCommandHandler> logger)
    : ICommandHandler<CreateBusinessCommand, BusinessDetailsDto>
{
    public async Task<BusinessDetailsDto> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        FileDataDto? fileData = null!;

        if (command.Data.Logo is not null)
        {
            fileData = await mediator.Send(new UploadFileCommand(command.Data.Logo), cancellationToken);
        }

        var dateNow = DateTime.Now;

        var business = new Business
        {
            OwnerId = command.UserId,
            Title = command.Data.Title,
            ShortDescription = command.Data.ShortDescription,
            FullDescription = command.Data.FullDescription,
            Address = command.Data.Address,
            Category = command.Data.Category,
            LogoId = fileData?.Id,
            CreatedDate = dateNow,
            UpdatedDate = dateNow
        };

        var businessId = await businessRepository.CreateBusinessAsync(business, cancellationToken);

        var galleryPhotos = new List<BusinessGalleryPhotoDto>();

        if (command.Data.GalleryPhotos is not null && command.Data.GalleryPhotos.Any())
        {
            galleryPhotos = await mediator.Send(new UploadBusinessGalleryPhotosCommand(command.Data.GalleryPhotos, businessId), cancellationToken);
        }

        return new BusinessDetailsDto(
            business.Id,
            business.OwnerId,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.LogoId is not null ? business.Logo?.Path : null,
            galleryPhotos,
            business.Address,
            business.ReviewsCount,
            (int)business.Category);
    }
}