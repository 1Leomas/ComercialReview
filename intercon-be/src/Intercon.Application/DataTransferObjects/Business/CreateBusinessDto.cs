﻿using Intercon.Domain.ComplexTypes;
using Intercon.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Intercon.Application.DataTransferObjects.Business;

public record CreateBusinessDto(
    string Title,
    string ShortDescription,
    string? FullDescription,
    IFormFile? Logo,
    IEnumerable<IFormFile>? GalleryPhotos,
    Address Address,
    BusinessCategory Category);