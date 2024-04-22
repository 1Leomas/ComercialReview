using Intercon.Domain.Abstractions;

namespace Intercon.Application.Abstractions;

public interface IImageValidator
{
    bool IsValidImage(IFileData file);
}