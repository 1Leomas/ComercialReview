using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions;

public static class BusinessMapper
{
    public static Business ToEntity(this CreateBusinessDto businessDto)
    {
        return new Business()
        {
            OwnerId = businessDto.OwnerId,
            Title = businessDto.Title,
            ShortDescription = businessDto.ShortDescription,
            FullDescription = businessDto.FullDescription,
            Logo = businessDto.Image,
            Address = businessDto.Address,
            Category = businessDto.Category
        };
    }

    public static BusinessDetailsDto ToDetailsDto(this Business business)
    {
        return new BusinessDetailsDto(
            business.Id,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.Address,
            business.ReviewsCount,
            business.Category);
    }
}