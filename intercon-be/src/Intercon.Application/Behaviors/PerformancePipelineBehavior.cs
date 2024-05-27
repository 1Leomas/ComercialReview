using Intercon.Application.Abstractions;
using Intercon.Application.PerformanceLogsManagement.GetPerformanceLogs;
using MediatR;

namespace Intercon.Application.Behaviors;

public class PerformancePipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : notnull
{
    public PerformancePipelineBehavior(IPerformanceLogRepository performanceLogRepository)
    {
        _performanceLogRepository = performanceLogRepository;
    }

    private readonly IPerformanceLogRepository _performanceLogRepository;

    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        TResponse? result = default;
        var startTime = DateTime.UtcNow;
        try
        {
            result = await next();
            var endTime = DateTime.UtcNow;

            if (request is not GetPerformanceLogsQuery)
                _performanceLogRepository.AddLogAsync(typeof(TRequest).Name, true, startTime, endTime,
                    CancellationToken.None);
        }
        catch (Exception e)
        {
            if (request is not GetPerformanceLogsQuery)
                _performanceLogRepository.AddLogAsync(typeof(TRequest).Name, false, startTime, DateTime.UtcNow,
                    CancellationToken.None);
            throw;
        }

        return result;
    }
}