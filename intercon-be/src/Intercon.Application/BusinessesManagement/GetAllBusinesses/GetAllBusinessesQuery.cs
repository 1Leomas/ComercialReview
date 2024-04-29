using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;

namespace Intercon.Application.BusinessesManagement.GetBusinesses;

public sealed record GetAllBusinessesQuery : IQuery<IEnumerable<BusinessShortDetailsDto>>;

internal sealed class GetAllBusinessesQueryHandler(IBusinessRepository businessRepository)
    : IQueryHandler<GetAllBusinessesQuery, IEnumerable<BusinessShortDetailsDto>>
{
    public async Task<IEnumerable<BusinessShortDetailsDto>> Handle(
        GetAllBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = await businessRepository.GetAllBusinessesAsync(cancellationToken);

        var businessesDetailsList = new List<BusinessShortDetailsDto>();

        foreach (var business in businesses)
        {
            var bDetails = new BusinessShortDetailsDto(
                business.Id,
                business.OwnerId,
                business.Title,
                business.ShortDescription,
                business.Rating,
                business.LogoId is not null ? business.Logo?.Path : null,
                business.Address,
                business.ReviewsCount,
                (int)business.Category);

            businessesDetailsList.Add(bDetails);
        }

        return businessesDetailsList;
    }
}