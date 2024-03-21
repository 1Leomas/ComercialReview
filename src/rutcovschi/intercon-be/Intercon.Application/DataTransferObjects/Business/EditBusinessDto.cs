namespace Intercon.Application.DataTransferObjects.Business;

public record EditBusinessDto(
    string? Title,
    string? ShortDescription,
    string? FullDescription,
    AddressDto? Address,
    ImageDto? Image);