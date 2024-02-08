using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Application.BusinessesManagement.GetBusinesses;

public sealed record GetAllBusinessesQuery : IQuery<IEnumerable<BusinessDetailsDto>>;

public sealed class GetAllBusinessesQueryHandler(InterconDbContext context) : IQueryHandler<GetAllBusinessesQuery, IEnumerable<BusinessDetailsDto>>
{
    private readonly InterconDbContext _context = context;

    public async Task<IEnumerable<BusinessDetailsDto>> Handle(
        GetAllBusinessesQuery query, CancellationToken cancellationToken)
    {
        //var businessesDb = await _context.Businesses.ToListAsync();
        return await _context.Businesses.Include(x => x.Logo).Select(b => b.ToDetailsDto()).ToListAsync();
    }
}
