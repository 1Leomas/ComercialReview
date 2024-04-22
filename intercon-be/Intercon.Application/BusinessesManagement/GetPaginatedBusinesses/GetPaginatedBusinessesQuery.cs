using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Pagination;

namespace Intercon.Application.BusinessesManagement.GetPaginatedBusinesses;

public sealed record GetPaginatedBusinessesQuery(BusinessParameters Parameters) : IQuery<PaginatedResponse<BusinessDetailsDto>>;

internal sealed class GetPaginatedBusinessesQueryHandler(IBusinessRepository businessRepository)
    : IQueryHandler<GetPaginatedBusinessesQuery, PaginatedResponse<BusinessDetailsDto>>
{
    public async Task<PaginatedResponse<BusinessDetailsDto>> Handle(
        GetPaginatedBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = 
            await businessRepository.GetPaginatedBusinessesAsync(query.Parameters, cancellationToken);

        var businessesDetailsList = new List<BusinessDetailsDto>();

        foreach (var business in businesses)
        {
            var bDetails = new BusinessDetailsDto(
                business.Id,
                business.OwnerId,
                business.Title,
                business.ShortDescription,
                business.FullDescription,
                business.Rating,
                business.LogoId is not null ? business.Logo?.Path : null,
                business.ProfileImages.Select(x => x.Path),
                business.Address,
                business.ReviewsCount,
                (int)business.Category);

            businessesDetailsList.Add(bDetails);
        }

        return new PaginatedResponse<BusinessDetailsDto>()
        {
            CurrentPage = businesses.CurrentPage,
            TotalPages = businesses.TotalPages,
            PageSize = businesses.PageSize,
            TotalCount = businesses.TotalCount,
            HasPrevious = businesses.HasPrevious,
            HasNext = businesses.HasNext,
            Items = businessesDetailsList
        };
    }
}