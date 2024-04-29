using Intercon.Application.PerformanceLogsManagement.GetPerformanceLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/performance-logs")]
[ApiController]
public class PerformanceLogs(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetPerformanceLogs(CancellationToken cancellationToken)
    {
        var performanceLogs = await _mediator.Send(new GetPerformanceLogsQuery(), cancellationToken);
        return Ok(performanceLogs);
    }
}