using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Business;

public record EditBusinessDto(
    string? Title,
    string? ShortDescription,
    string? FullDescription, 
    BusinessCategory Category,
    AddressDto? Address);