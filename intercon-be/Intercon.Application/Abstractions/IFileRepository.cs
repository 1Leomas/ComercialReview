using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions;

public interface IFileRepository
{
    Task<FileData?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<FileData> UploadFileAsync(string filePath, CancellationToken cancellationToken);
    Task<FileData> UploadFileAsync(string filePath, int businessId, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}