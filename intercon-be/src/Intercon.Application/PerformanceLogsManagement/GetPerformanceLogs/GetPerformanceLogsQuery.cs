using Intercon.Application.Abstractions;
using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.DataTransferObjects;
using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;

namespace Intercon.Application.PerformanceLogsManagement.GetPerformanceLogs;

public sealed record GetPerformanceLogsQuery(PerformanceLogsParameters Parameters) : IQuery<PaginatedResponse<PerformanceLog>>;

internal sealed class GetPerformanceLogsQueryHandler : IQueryHandler<GetPerformanceLogsQuery, PaginatedResponse<PerformanceLog>>
{
    private readonly IPerformanceLogRepository _performanceLogRepository;

    public GetPerformanceLogsQueryHandler(IPerformanceLogRepository performanceLogRepository)
    {
        _performanceLogRepository = performanceLogRepository;
    }

    public async Task<PaginatedResponse<PerformanceLog>> Handle(GetPerformanceLogsQuery request,
        CancellationToken cancellationToken)
    {
        var logs = await _performanceLogRepository.GetAllAsync(request.Parameters, cancellationToken);

        return new PaginatedResponse<PerformanceLog>()
        {
            CurrentPage = logs.CurrentPage,
            TotalPages = logs.TotalPages,
            PageSize = logs.PageSize,
            TotalCount = logs.TotalCount,
            HasPrevious = logs.HasPrevious,
            HasNext = logs.HasNext,
            Items = logs
        };
    }
}