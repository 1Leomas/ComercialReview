using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Entities;
using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Business;

public record CreateBusinessDto(
    int OwnerId,
    string Title,
    string ShortDescription,
    string? FullDescription,
    Image? Image,
    Address Address,
    BusinessCategory Category);
