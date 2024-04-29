using Microsoft.AspNetCore.Http;

namespace Intercon.Application.DataTransferObjects.Files;

public class FileUploadModel
{
    public IFormFile ImageFile { get; set; } = null!;
}