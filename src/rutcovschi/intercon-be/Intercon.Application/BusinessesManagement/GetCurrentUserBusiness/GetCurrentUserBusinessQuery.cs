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

        return business?.ToDetailsDto();
    }
}
