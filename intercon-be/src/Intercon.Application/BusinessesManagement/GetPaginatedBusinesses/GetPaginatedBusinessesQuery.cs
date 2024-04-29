using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Pagination;

namespace Intercon.Application.BusinessesManagement.GetPaginatedBusinesses;

public sealed record GetPaginatedBusinessesQuery(BusinessParameters Parameters) : IQuery<PaginatedResponse<BusinessShortDetailsDto>>;

internal sealed class GetPaginatedBusinessesQueryHandler(IBusinessRepository businessRepository)
    : IQueryHandler<GetPaginatedBusinessesQuery, PaginatedResponse<BusinessShortDetailsDto>>
{
    public async Task<PaginatedResponse<BusinessShortDetailsDto>> Handle(
        GetPaginatedBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = 
            await businessRepository.GetPaginatedBusinessesAsync(query.Parameters, cancellationToken);

        var businessesDetailsList = new List<BusinessShortDetailsDto>();

        foreach (var business in businesses)
        {
            var businessShortDetailsDto = new BusinessShortDetailsDto(
                business.Id,
                business.OwnerId,
                business.Title,
                business.ShortDescription,
                business.Rating,
                business.LogoId is not null ? business.Logo?.Path : null,
                business.Address,
                business.ReviewsCount,
                (int)business.Category);

            businessesDetailsList.Add(businessShortDetailsDto);
        }

        return new PaginatedResponse<BusinessShortDetailsDto>()
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