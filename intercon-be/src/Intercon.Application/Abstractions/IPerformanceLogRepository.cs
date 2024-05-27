using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;

namespace Intercon.Application.Abstractions;

public interface IPerformanceLogRepository
{
    Task<PaginatedList<PerformanceLog>> GetAllAsync(PerformanceLogsParameters parameters, CancellationToken cancellationToken);
    Task<bool> AddLogAsync(PerformanceLog log, CancellationToken cancellationToken);
    Task<bool> AddLogAsync(string requestName, bool isSuccess, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
}