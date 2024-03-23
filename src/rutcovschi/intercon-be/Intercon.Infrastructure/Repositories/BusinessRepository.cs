﻿using Intercon.Application.Abstractions;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class BusinessRepository(InterconDbContext context) 
    : IBusinessRepository
{
    public async Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Businesses
            .AsNoTracking()
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Business>> GetAllBusinessesAsync(CancellationToken cancellationToken)
    {
        return await context.Businesses
            .Include(x => x.Logo)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CreateBusinessAsync(Business newBusiness, CancellationToken cancellationToken)
    {
        await context.Businesses.AddAsync(newBusiness, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return newBusiness.Id;
    }

    public async Task<Business?> UpdateBusinessAsync(int id, EditBusinessDto newBusinessData, CancellationToken cancellationToken)
    {
        var businessDb = await context.Businesses
            .Include(business => business.Logo)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (businessDb == null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(newBusinessData.Title))
        {
            businessDb.Title = newBusinessData.Title;
        }

        if (!string.IsNullOrEmpty(newBusinessData.ShortDescription))
        {
            businessDb.ShortDescription = newBusinessData.ShortDescription;
        }

        if (!string.IsNullOrEmpty(newBusinessData.ShortDescription))
        {
            businessDb.FullDescription = newBusinessData.FullDescription;
        }

        if (newBusinessData.Address != null)
        {
            businessDb.Address.Street = newBusinessData.Address.Street ?? businessDb.Address.Street;
            businessDb.Address.Latitude = newBusinessData.Address.Latitude ?? businessDb.Address.Latitude;
            businessDb.Address.Longitude = newBusinessData.Address.Longitude ?? businessDb.Address.Longitude;
        }

        if (newBusinessData.Image != null)
        {
            businessDb.Logo ??= new Image();
            businessDb.Logo.Data = newBusinessData.Image.Data ?? businessDb.Logo.Data;
        }

        businessDb.UpdateDate = DateTime.Now;
        businessDb.WasEdited = true;

        await context.SaveChangesAsync(cancellationToken);

        return businessDb;
    }

    public async Task<bool> BusinessExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Businesses.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> UserHasBusinessAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Businesses.AllAsync(x => x.OwnerId != userId, cancellationToken);
    }
}