﻿using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.CreateBusiness;

public sealed record CreateBusinessCommand(int UserId, CreateBusinessDto Data) : ICommand<BusinessDetailsDto>;

public sealed class CreateBusinessCommandHandler(InterconDbContext context, UserManager<User> userManager) : ICommandHandler<CreateBusinessCommand, BusinessDetailsDto>
{
    private readonly InterconDbContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<BusinessDetailsDto> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
    {
        int? logoId = null;

        // maybe make a separate request for this
        Image? image = null;
        if (command.Data.Logo != null)
        {
            image = new Image()
            {
                Data = command.Data.Logo.Data
            };

            await _context.Images.AddAsync(image, cancellationToken);
            var rows = await _context.SaveChangesAsync(cancellationToken);

            logoId = rows != 0 ? image.Id : null!;
        }

        var businessDb = new Business()
        {
            OwnerId = command.UserId,
            Title = command.Data.Title,
            ShortDescription = command.Data.ShortDescription,
            FullDescription = command.Data.FullDescription,
            Address = command.Data.Address,
            Category = command.Data.Category,
            LogoId = logoId
        };

        await _context.Businesses.AddAsync(businessDb, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new BusinessDetailsDto(
            Id: businessDb.Id,
            Title: businessDb.Title,
            ShortDescription: businessDb.ShortDescription,
            FullDescription: businessDb.FullDescription,
            Rating: businessDb.Rating,
            Logo: businessDb.LogoId.HasValue ? new ImageDto(Data: image!.Data) : null,
            Address: businessDb.Address,
            ReviewsCount: businessDb.ReviewsCount,
            Category: businessDb.Category
        );
    }
}