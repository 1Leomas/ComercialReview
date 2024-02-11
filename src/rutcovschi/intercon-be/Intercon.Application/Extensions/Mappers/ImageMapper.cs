using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ImageMapper
{
    public static Image ToEntity(this CreateImageDto imageDto)
    {
        return new Image()
        {
            //ContentType = imageDto.ContentType,
            Data = imageDto.Data
        };
    }

    public static CreateImageDto ToDto(this Image? image)
    {
        if (image is null)
        {
            return null!;
        }

        return new CreateImageDto(
            //image.ContentType,
            image.Data
        );
    }
}