namespace Intercon.Domain.Entities;

public class Image
{
    public int Id { get; set; }
    public string ContentType { get; set; } = null!;
    public byte[] Raw { get; set; } = null!;
}