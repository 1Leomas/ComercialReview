using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Business;

public sealed class EditBusinessDetailsDto
{
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string? FullDescription { get; set; } = null!;
    public BusinessCategory Category { get; set; }
    public AddressDto Address { get; set; } = null!;
    public string? LogoPath { get; set; } = null!;
}