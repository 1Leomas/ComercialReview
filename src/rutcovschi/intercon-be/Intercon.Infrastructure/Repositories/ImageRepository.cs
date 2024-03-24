using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;

namespace Intercon.Infrastructure.Repositories;

public class ImageRepository(InterconDbContext context)
    : IImageRepository
{
    public async Task<int?> AddImage(FileData fileData, CancellationToken cancellationToken)
    {
        var avatar = new FileData()
        {
            Path = fileData.Path
        };

        await context.DataFiles.AddAsync(avatar, cancellationToken);
        var rows = await context.SaveChangesAsync(cancellationToken);

        return rows == 0 ? null : avatar.Id;
    }
}