using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions.Mappers;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.GetBusiness;

public sealed record GetBusinessQuery(int Id) : IQuery<BusinessDetailsDto?>;

public sealed class GetBusinessQueryHandler(InterconDbContext context) : IQueryHandler<GetBusinessQuery, BusinessDetailsDto?>
{
    private readonly InterconDbContext _context = context;

    public async Task<BusinessDetailsDto?> Handle(GetBusinessQuery query, CancellationToken cancellationToken)
    {
        var business = await _context.Businesses
            .Include(business => business.Logo)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (business == null)
        {
            return null;
        }

        return business.ToDetailsDto();
    }
}
