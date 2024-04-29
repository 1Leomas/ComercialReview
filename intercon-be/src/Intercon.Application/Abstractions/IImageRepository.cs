using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IImageRepository
{
    Task<int?> AddImage(FileData fileData, CancellationToken cancellationToken);
}