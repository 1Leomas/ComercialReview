using Intercon.Application.Abstractions;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CA1862

namespace Intercon.Infrastructure.Repositories;

public class BusinessRepository(InterconDbContext context) 
    : IBusinessRepository
{
    public async Task<Business?> GetBusinessByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Businesses
            .AsNoTracking()
            .Include(x => x.Logo)
            .Include(x => x.GalleryPhotos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Business?> GetBusinessByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Businesses
            .AsNoTracking()
            .Include(x => x.Logo)
            .Include(x => x.GalleryPhotos)
            .FirstOrDefaultAsync(x => x.OwnerId == userId, cancellationToken);
    }

    public async Task<IEnumerable<Business>> GetAllBusinessesAsync(CancellationToken cancellationToken)
    {
        return await context.Businesses
            .Include(x => x.Logo)
            .Include(x => x.GalleryPhotos)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<Business>> GetPaginatedBusinessesAsync(BusinessParameters parameters, CancellationToken cancellationToken)
    {
        var businesses = context.Businesses
            .AsNoTracking()
            .Include(x => x.Logo)
            .Include(x => x.GalleryPhotos)
            .AsQueryable();

        businesses = ApplyFilter(businesses, parameters);

        businesses = ApplySort(businesses, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<Business>
            .ToPagedList(businesses, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<int> CreateBusinessAsync(Business newBusiness, CancellationToken cancellationToken)
    {
        await context.Businesses.AddAsync(newBusiness, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return newBusiness.Id;
    }

    public async Task<Business?> UpdateBusinessAsync(int id, EditBusinessRequest newBusinessData, int? logoId, CancellationToken cancellationToken)
    {
        var businessDb = await context.Businesses
            .Include(business => business.Logo)
            .Include(x => x.GalleryPhotos)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (businessDb == null) return null;

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
        if (newBusinessData.Category.HasValue)
        {
            businessDb.Category = newBusinessData.Category.Value;
        }
        if (logoId.HasValue)
        {
            businessDb.LogoId = logoId.Value;
        }

        businessDb.UpdatedDate = DateTime.Now;

        await context.SaveChangesAsync(cancellationToken);

        return businessDb;
    }

    public async Task<bool> BusinessExistsAsync(int id, CancellationToken cancellationToken)
    {
        return await context.Businesses.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> UserHasBusinessAsync(int userId, CancellationToken cancellationToken)
    {
        return await context.Businesses.AnyAsync(x => x.OwnerId == userId, cancellationToken);
    }

    public async Task<int?> GetBusinessLogoIdAsync(int businessId, CancellationToken cancellationToken)
    {
        return await context.Businesses
            .Where(x => x.Id == businessId)
            .Select(x => x.LogoId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UserOwnsBusinessAsync(int userId, int businessId, CancellationToken cancellationToken)
    {
        return await context.Businesses
            .AnyAsync(x => x.OwnerId == userId && x.Id == businessId, cancellationToken);
    }

    private static IQueryable<Business> ApplySort(
        IQueryable<Business> businesses,
        BusinessSortBy sortBy,
        SortingDirection? direction = SortingDirection.Ascending)
    {
        return sortBy switch
        {
            BusinessSortBy.UpdatedDate => businesses.OrderUsing(x => x.UpdatedDate, direction ?? SortingDirection.Descending),
            BusinessSortBy.Title => businesses.OrderUsing(x => x.Title, direction ?? SortingDirection.Ascending),
            BusinessSortBy.Category => businesses.OrderUsing(x => x.Category, direction ?? SortingDirection.Ascending),
            BusinessSortBy.Rating => businesses.OrderUsing(x => x.Rating, direction ?? SortingDirection.Ascending),
            _ => businesses.OrderUsing(x => x.UpdatedDate, SortingDirection.Descending)
        };
    }

    private static IQueryable<Business> ApplyFilter(IQueryable<Business> businesses, BusinessParameters parameters)
    {

        if (!string.IsNullOrEmpty(parameters.Search))
        {
            var search = parameters.Search.ToLower();

            businesses = businesses.Where(x =>
                x.Title.ToLower().Contains(search) ||
                x.ShortDescription.ToLower().Contains(search) ||
                (x.FullDescription != null && x.FullDescription.ToLower().Contains(search)) ||
                (x.Address.Street != null && x.Address.Street.ToLower().Contains(search)));
        }

        if (parameters.Categories.Any() && !parameters.Categories.Contains(BusinessCategory.All))
        {
            businesses = businesses.Where(x => parameters.Categories.Contains(x.Category));
        }

        businesses = businesses.Where(x => x.Rating >= parameters.MinGrade && x.Rating <= parameters.MaxGrade);

        return businesses;
    }
}