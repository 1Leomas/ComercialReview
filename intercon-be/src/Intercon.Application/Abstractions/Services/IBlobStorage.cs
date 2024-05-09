using Microsoft.AspNetCore.Http;

namespace Intercon.Application.Abstractions.Services;

public interface IBlobStorage
{
    Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken);
}