using Intercon.Application.DataTransferObjects.Files;
using Intercon.Domain.ComplexTypes;

namespace Intercon.Application.DataTransferObjects.Business;

public record BusinessDetailsDto(
    int Id,
    int OwnerId,
    string Title,
    string ShortDescription,
    string? FullDescription,
    float Rating,
    string? LogoPath,
    IEnumerable<BusinessGalleryPhotoDto> GalleryPhotos,
    Address Address,
    uint ReviewsCount,
    int Category);