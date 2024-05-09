using Intercon.Application.Abstractions.Messaging;
using Intercon.Application.Abstractions.Repositories;
using Intercon.Domain.Entities;

namespace Intercon.Application.PerformanceLogsManagement.Add;

public sealed record AddPerformanceLogCommand(string RequestName, DateTime StartTime, DateTime EndTime) : ICommand;

internal sealed class AddPerformanceLogCommandHandler : ICommandHandler<AddPerformanceLogCommand>
{
    private readonly IPerformanceLogRepository _performanceLogRepository;

    public AddPerformanceLogCommandHandler(IPerformanceLogRepository performanceLogRepository)
    {
        _performanceLogRepository = performanceLogRepository;
    }

    public async Task Handle(AddPerformanceLogCommand request, CancellationToken cancellationToken)
    {
        var log = new PerformanceLog
        {
            RequestName = request.RequestName,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            RequestDuration = request.EndTime - request.StartTime,
        };

        await _performanceLogRepository.AddLogAsync(log, cancellationToken);
    }
}