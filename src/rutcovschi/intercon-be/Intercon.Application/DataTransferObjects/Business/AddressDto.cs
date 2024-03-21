namespace Intercon.Application.DataTransferObjects.Business;

public record AddressDto
{
    public string? Street { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
};