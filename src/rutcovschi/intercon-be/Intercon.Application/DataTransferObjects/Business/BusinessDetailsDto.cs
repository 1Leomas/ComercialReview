using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Enums;

namespace Intercon.Application.DataTransferObjects.Business;

public record BusinessDetailsDto(
    int Id,
    string Title,
    string ShortDescription,
    string? FullDescription,
    float Rating,
    Address Address,
    uint ReviewsCount,
    BusinessCategory Category);
