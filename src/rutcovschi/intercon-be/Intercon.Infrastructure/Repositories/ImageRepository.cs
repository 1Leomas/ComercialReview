using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Infrastructure.Repositories;

public class ImageRepository(InterconDbContext context)
    : IImageRepository
{
    public async Task<int?> AddImage(Image image, CancellationToken cancellationToken)
    {
        var avatar = new Image()
        {
            Data = image.Data
        };

        await context.Images.AddAsync(avatar, cancellationToken);
        var rows = await context.SaveChangesAsync(cancellationToken);

        return rows == 0 ? null : avatar.Id;
    }
}