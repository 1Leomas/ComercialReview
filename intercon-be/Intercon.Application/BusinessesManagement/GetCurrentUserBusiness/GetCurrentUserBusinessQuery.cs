using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.BusinessesManagement.GetCurrentUserBusiness;

public sealed record GetCurrentUserBusinessQuery(int UserId) : IQuery<BusinessDetailsDto?>;

public sealed class GetCurrentUserBusinessQueryHandler
    (IBusinessRepository businessRepository) : IQueryHandler<GetCurrentUserBusinessQuery, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(GetCurrentUserBusinessQuery request,
        CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetBusinessByUserIdAsync(request.UserId, cancellationToken);

        if (business is null)
        {
            return null;
        }

        return new BusinessDetailsDto(
            business.Id,
            business.OwnerId,
            business.Title,
            business.ShortDescription,
            business.FullDescription,
            business.Rating,
            business.LogoId is not null ? business.Logo?.Path : null,
            business.PhotoGallery.Select(x => x.Path),
            business.Address,
            business.ReviewsCount,
            (int)business.Category);
    }
}
