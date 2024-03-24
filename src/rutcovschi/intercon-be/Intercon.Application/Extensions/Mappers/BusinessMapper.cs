using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class BusinessMapper
{
    public static Business ToEntity(this CreateBusinessDto businessDto)
    {
        return new Business
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
            business.OwnerId,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.LogoId is not null ? business.Logo?.Path : null,
            business.Address,
            business.ReviewsCount,
            business.Category);
    }
}