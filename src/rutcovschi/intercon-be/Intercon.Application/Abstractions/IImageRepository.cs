using Intercon.Application.DataTransferObjects;

namespace Intercon.Application.Abstractions;

public interface IImageRepository
{
    bool AddImage(ImageDto image);
}
