using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class FileRepository(InterconDbContext context) : IFileRepository
{
    private readonly InterconDbContext _context = context;

    public async Task<FileData?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.DataFiles
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FileData> UploadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        var fileData = new FileData
        {
            Path = filePath,
        };

        _context.DataFiles.Add(fileData);

        await _context.SaveChangesAsync(cancellationToken);

        return fileData;
    }

    public async Task<FileData> UploadFileAsync(string filePath, int businessId, CancellationToken cancellationToken)
    {
        var fileData = new FileData
        {
            Path = filePath,
            BusinessId = businessId
        };

        _context.DataFiles.Add(fileData);

        await _context.SaveChangesAsync(cancellationToken);

        return fileData;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var rows = await _context.DataFiles
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return rows > 0;
    }
}