using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class PerformanceLogRepository(InterconDbContext context) : IPerformanceLogRepository
{
    public async Task<IEnumerable<PerformanceLog>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.PerformanceLogs.ToListAsync(cancellationToken);
    }

    public async Task<bool> AddLogAsync(PerformanceLog log, CancellationToken cancellationToken)
    {
        await context.PerformanceLogs.AddAsync(log, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddLogAsync(string requestName, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
    {
        var log = new PerformanceLog
        {
            RequestName = requestName,
            StartTime = startTime,
            EndTime = endTime,
            RequestDuration = endTime - startTime,
        };

        return await AddLogAsync(log, cancellationToken);
    }
}