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
        var startTime = DateTime.Now;
        try
        {
            result = await next();
            var endTime = DateTime.Now;

            if (request is not GetPerformanceLogsQuery)
                _performanceLogRepository.AddLogAsync(typeof(TRequest).Name, true, startTime, endTime,
                    CancellationToken.None);
        }
        catch (Exception)
        {
            if (request is not GetPerformanceLogsQuery)
                _performanceLogRepository.AddLogAsync(typeof(TRequest).Name, false, startTime, DateTime.Now,
                    CancellationToken.None);
        }

        return result;
    }
}