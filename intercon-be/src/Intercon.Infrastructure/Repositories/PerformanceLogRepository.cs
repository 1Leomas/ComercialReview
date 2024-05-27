using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

public class PerformanceLogRepository(InterconDbContext context) : IPerformanceLogRepository
{
    public async Task<PaginatedList<PerformanceLog>> GetAllAsync(PerformanceLogsParameters parameters, CancellationToken cancellationToken)
    {
        var logs = context.PerformanceLogs
            .AsNoTracking()
            .AsQueryable();

        logs = ApplyFilter(logs, parameters);

        logs = ApplySort(logs, parameters.SortBy, parameters.SortDirection);

        return await PaginatedList<PerformanceLog>
            .ToPagedList(logs, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<bool> AddLogAsync(PerformanceLog log, CancellationToken cancellationToken)
    {
        await context.PerformanceLogs.AddAsync(log, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> AddLogAsync(string requestName, bool isSuccess, DateTime startTime, DateTime endTime, CancellationToken cancellationToken)
    {
        var log = new PerformanceLog
        {
            RequestName = requestName,
            IsSuccess = isSuccess,
            StartTime = startTime,
            EndTime = endTime,
            RequestDuration = endTime - startTime,
        };

        return await AddLogAsync(log, cancellationToken);
    }

    private static IQueryable<PerformanceLog> ApplyFilter(IQueryable<PerformanceLog> logs, PerformanceLogsParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.Search))
        {
            var search = parameters.Search.ToLower();

            logs = logs.Where(x =>
                x.RequestName.ToLower().Contains(search));
        }

        if (parameters.IsSuccess.HasValue)
        {
            logs = logs.Where(x => x.IsSuccess == parameters.IsSuccess);
        }

        return logs;
    }

    private static IQueryable<PerformanceLog> ApplySort(
        IQueryable<PerformanceLog> logs,
        PerformanceLogsSortBy sortBy,
        SortingDirection? direction = SortingDirection.Ascending)
    {
        return sortBy switch
        {
            PerformanceLogsSortBy.StartTime => logs.OrderUsing(x => x.StartTime, direction ?? SortingDirection.Descending),
            PerformanceLogsSortBy.RequestDuration => logs.OrderUsing(x => x.RequestDuration, direction ?? SortingDirection.Descending),
            PerformanceLogsSortBy.RequestName => logs.OrderUsing(x => x.RequestName, direction ?? SortingDirection.Ascending),
            _ => logs.OrderUsing(x => x.StartTime, SortingDirection.Descending)
        };
    }
}