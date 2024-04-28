using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Business;

public record BusinessDetailsDto(
    int Id,
    int OwnerId,
    string Title,
    string ShortDescription,
    string? FullDescription,
    float Rating,
    string? LogoPath,
    IEnumerable<string> PhotoGallery,
    Address Address,
    uint ReviewsCount,
    int Category);