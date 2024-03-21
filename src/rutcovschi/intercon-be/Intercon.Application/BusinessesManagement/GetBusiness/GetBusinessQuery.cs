using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.BusinessesManagement.GetBusiness;

public sealed record GetBusinessQuery(int Id) : IQuery<BusinessDetailsDto?>;

public sealed class GetBusinessQueryHandler(IBusinessRepository businessRepository) : IQueryHandler<GetBusinessQuery, BusinessDetailsDto?>
{
    public async Task<BusinessDetailsDto?> Handle(GetBusinessQuery query, CancellationToken cancellationToken)
    {
        var business = await businessRepository.GetBusinessByIdAsync(query.Id, cancellationToken);

        return business?.ToDetailsDto();
    }
}
