using Intercon.Domain.Abstractions;

namespace Intercon.Application.DataTransferObjects.Files;

public class ImageData : IFileData
{
    public string ContentType { get; set; }
    public byte[] Raw { get; set; }

    public ImageData(string contentType, byte[] raw)
    {
        ContentType = contentType;
        Raw = raw;
    }
}