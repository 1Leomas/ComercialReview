using Microsoft.AspNetCore.Http;

namespace Intercon.Application.Extensions;

public static class FormFileExtensions
{
    public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}