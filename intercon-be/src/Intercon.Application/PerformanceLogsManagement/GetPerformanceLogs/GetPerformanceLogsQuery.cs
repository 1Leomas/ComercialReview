using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Entities;

namespace Intercon.Application.PerformanceLogsManagement.GetPerformanceLogs;

public sealed record GetPerformanceLogsQuery : IQuery<IEnumerable<PerformanceLog>>;

internal sealed class GetPerformanceLogsQueryHandler : IQueryHandler<GetPerformanceLogsQuery, IEnumerable<PerformanceLog>>
{
    private readonly IPerformanceLogRepository _performanceLogRepository;

    public GetPerformanceLogsQueryHandler(IPerformanceLogRepository performanceLogRepository)
    {
        _performanceLogRepository = performanceLogRepository;
    }

    public async Task<IEnumerable<PerformanceLog>> Handle(GetPerformanceLogsQuery request,
        CancellationToken cancellationToken)
    {
        return await _performanceLogRepository.GetAllAsync(cancellationToken);
    }
}