using System.ComponentModel.DataAnnotations.Schema;

namespace Intercon.Domain.ComplexTypes;

[ComplexType]
public record Address(string Street, string Latitude, string Longitude);