using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IImageRepository
{
    Task<int?> AddImage(Image image, CancellationToken cancellationToken);
}