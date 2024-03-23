using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ImageMapper
{
    public static Image ToEntity(this CreateImageDto imageDto)
    {
        return new Image
        {
            //ContentType = imageDto.ContentType,
            Data = imageDto.Data
        };
    }

    public static CreateImageDto ToCreateImageDto(this Image image)
    {
        return new CreateImageDto(image.Data);
    }

    public static ImageDto ToDto(this Image image)
    {
        return new ImageDto(image.Data);
    }
}