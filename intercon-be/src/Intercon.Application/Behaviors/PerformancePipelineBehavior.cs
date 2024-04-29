using Intercon.Application.Abstractions;
using Intercon.Application.PerformanceLogsManagement.Add;
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
        var startTime = DateTime.Now;
        var result = await next();
        var endTime = DateTime.Now;

        _performanceLogRepository.AddLogAsync(typeof(TRequest).Name, startTime, endTime, CancellationToken.None);

        return result;
    }
}