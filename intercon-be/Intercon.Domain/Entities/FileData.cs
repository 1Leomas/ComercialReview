using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class FileData
{
    public int Id { get; set; }
    public string Path { get; set; } = null!;

    public int? BusinessId { get; set; }

    public virtual Business Business { get; set; } = null!;
}