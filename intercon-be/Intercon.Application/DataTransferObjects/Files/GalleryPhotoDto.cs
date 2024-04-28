namespace Intercon.Application.DataTransferObjects.Files;

public class GalleryPhotoDto
{
    public int Id { get; set; }
    public string Path { get; set; } = null!;
    public int BusinessId { get; set; }
}