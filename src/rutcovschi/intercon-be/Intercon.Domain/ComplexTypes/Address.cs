using System.ComponentModel.DataAnnotations.Schema;

namespace Intercon.Domain.ComplexTypes;

[ComplexType]
public record Address
{
    public string? Street { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
};