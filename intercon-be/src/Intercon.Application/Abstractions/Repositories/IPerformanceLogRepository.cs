using Intercon.Domain.Entities;

namespace Intercon.Application.Abstractions.Repositories;

public interface IPerformanceLogRepository
{
    Task<IEnumerable<PerformanceLog>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> AddLogAsync(PerformanceLog log, CancellationToken cancellationToken);
    Task<bool> AddLogAsync(string requestName, DateTime startTime, DateTime endTime, CancellationToken cancellationToken);
}