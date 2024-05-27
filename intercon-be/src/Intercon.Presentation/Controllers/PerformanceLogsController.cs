using Intercon.Application.DataTransferObjects;
using Intercon.Application.PerformanceLogsManagement.GetPerformanceLogs;
using Intercon.Domain.Entities;
using Intercon.Domain.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/performance-logs")]
[ApiController]
public class PerformanceLogsController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<PerformanceLog>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPerformanceLogs([FromQuery] PerformanceLogsParameters parameters, CancellationToken cancellationToken)
    {
        var performanceLogs = 
            await _mediator.Send(new GetPerformanceLogsQuery(parameters), cancellationToken);
        return Ok(performanceLogs);
    }
}