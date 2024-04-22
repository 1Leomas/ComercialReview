using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Entities;

namespace Intercon.Application.Extensions.Mappers;

public static class ImageMapper
{
    public static FileData ToEntity(this CreateImageDto imageDto)
    {
        return new FileData
        {
            //ContentType = imageDto.ContentType,
            Path = imageDto.Data
        };
    }

    public static CreateImageDto ToCreateImageDto(this FileData fileData)
    {
        return new CreateImageDto(fileData.Path);
    }

    public static FileDataDto ToDto(this FileData fileData)
    {
        return new FileDataDto(fileData.Id, fileData.Path);
    }
}