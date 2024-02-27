using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class BusinessMapper
{
    public static Business ToEntity(this CreateBusinessDto businessDto)
    {
        return new Business()
        {
            Title = businessDto.Title,
            ShortDescription = businessDto.ShortDescription,
            FullDescription = businessDto.FullDescription,
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
            business.Logo?.ToDto(),
            business.Address,
            business.ReviewsCount,
            business.Category);
    }
}