using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;

namespace Intercon.Application.BusinessesManagement.GetBusinesses;

public sealed record GetAllBusinessesQuery : IQuery<IEnumerable<BusinessDetailsDto>>;

internal sealed class GetAllBusinessesQueryHandler(IBusinessRepository businessRepository)
    : IQueryHandler<GetAllBusinessesQuery, IEnumerable<BusinessDetailsDto>>
{
    public async Task<IEnumerable<BusinessDetailsDto>> Handle(
        GetAllBusinessesQuery query, CancellationToken cancellationToken)
    {
        var businesses = await businessRepository.GetAllBusinessesAsync(cancellationToken);

        return businesses.Select(b => b.ToDetailsDto());
    }
}