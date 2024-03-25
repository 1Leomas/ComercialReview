using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.DataTransferObjects.Business;

public record EditBusinessRequest(
    string? Title,
    string? ShortDescription,
    string? FullDescription, 
    BusinessCategory? Category,
    AddressDto? Address,
    IFormFile? Logo);