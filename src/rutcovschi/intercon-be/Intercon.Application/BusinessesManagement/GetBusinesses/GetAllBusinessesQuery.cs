using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;

namespace Intercon.Application.BusinessesManagement.GetBusinesses;

public sealed record GetAllBusinessesQuery : IQuery<IEnumerable<BusinessDetailsDto>>;

internal sealed class GetAllBusinessesQueryHandler(IBusinessRepository businessRepository)
    : IQueryHandler<GetAllBusinessesQuery, IEnumerable<BusinessDetailsDto>>
{
    public async Task<IEnumerable<BusinessDetailsDto>> Handle(
        GetAllBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = await businessRepository.GetAllBusinessesAsync(cancellationToken);

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

        return businessesDetailsList;
    }
}