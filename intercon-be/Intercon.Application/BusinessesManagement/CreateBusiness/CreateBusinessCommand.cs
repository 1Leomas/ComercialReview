using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;
using Intercon.Application.FilesManagement.UploadBusinessProfileImages;
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
        
        var profileImages = new List<string>();

        if (command.Data.ProfileImages is not null && command.Data.ProfileImages.Any())
        {
            profileImages = await mediator.Send(new UploadBusinessProfileImagesCommand(command.Data.ProfileImages, businessId), cancellationToken);
        }

        return new BusinessDetailsDto(
            business.Id,
            business.OwnerId,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.LogoId is not null ? business.Logo?.Path : null,
            profileImages,
            business.Address,
            business.ReviewsCount,
            (int)business.Category);
    }
}