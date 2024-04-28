using Intercon.Application.DataTransferObjects.Files;
using Intercon.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.Abstractions;

public interface IFileRepository
{
    Task<FileData?> UploadFileAsync(IFormFile imageData, CancellationToken cancellationToken);
    Task<FileData?> UploadFileAsync(IFormFile imageData, int businessId, CancellationToken cancellationToken);
    Task DeleteFileAsync(int id, CancellationToken cancellationToken);
}