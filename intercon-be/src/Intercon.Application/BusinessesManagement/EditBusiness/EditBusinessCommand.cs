﻿using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.DataTransferObjects.Files;
using Intercon.Application.FilesManagement.DeleteFile;
using Intercon.Application.FilesManagement.UploadBusinessProfileImages;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Domain.Entities;
using MediatR;

namespace Intercon.Application.BusinessesManagement.EditBusiness;

public sealed record EditBusinessCommand(
        int CurrentUserId, 
        int BusinessId, 
        EditBusinessRequest Data, int? NewLogoId) 
    : ICommand<BusinessDetailsDto?>;

public sealed class EditBusinessCommandHandler(
    IBusinessRepository businessRepository,
    IMediator mediator) : ICommandHandler<EditBusinessCommand, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(EditBusinessCommand command, CancellationToken cancellationToken)
    {
        // TO DO: check if the user is the owner of the business


        if (command.NewLogoId is not null)
        {
            var businessOldLogoId = 
                await businessRepository.GetBusinessLogoIdAsync(command.BusinessId, cancellationToken);

            if (businessOldLogoId is not null)
                await mediator.Send(new DeleteFileQuery(businessOldLogoId.Value), cancellationToken);
        }

        var profileImages = new List<BusinessGalleryPhotoDto>();

        if (command.Data.NewProfileImages is not null && command.Data.NewProfileImages.Any())
        {
            profileImages.AddRange(await mediator.Send(
                new UploadBusinessProfileImagesCommand(
                    command.Data.NewProfileImages, command.BusinessId),
                        cancellationToken));
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
            profileImages,
            business.Address,
            business.ReviewsCount,
            (int)business.Category);
    }
}