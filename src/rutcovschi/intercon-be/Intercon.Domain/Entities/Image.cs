using Intercon.Domain.Abstractions;

namespace Intercon.Domain.Entities;

public class Image : Entity
{
    //public IFileData Data { get; set; } = null!;
    public string Data { get; set; } = null!;
}