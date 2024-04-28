using Intercon.Domain.ComplexTypes;

namespace Intercon.Application.DataTransferObjects.Business;

public record BusinessShortDetailsDto(
    int Id,
    int OwnerId,
    string Title,
    string ShortDescription,
    float Rating,
    string? LogoPath,
    Address Address,
    uint ReviewsCount,
    int Category);