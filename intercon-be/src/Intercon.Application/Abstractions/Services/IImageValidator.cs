using Intercon.Domain.Abstractions;

namespace Intercon.Application.Abstractions.Services;

public interface IImageValidator
{
    bool IsValidImage(IFileData file);
}