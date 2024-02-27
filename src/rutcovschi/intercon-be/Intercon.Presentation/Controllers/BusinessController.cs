using Intercon.Application.BusinessesManagement.CreateBusiness;
using Intercon.Application.BusinessesManagement.EditBusiness;
using Intercon.Application.BusinessesManagement.GetBusiness;
using Intercon.Application.BusinessesManagement.GetBusinesses;
using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/businesses")]
[ApiController]
public class BusinessController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBusiness([FromRoute] int id, CancellationToken cancellationToken)
    {
        var business = await _mediator.Send(new GetBusinessQuery(id), cancellationToken);

        if (business == null)
        {
            return NotFound();
        }

        return Ok(business);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BusinessDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinesses(CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetAllBusinessesQuery(), cancellationToken));
    }

    [Authorize(Roles = "2")]
    [HttpPost]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessDto businessToAdd, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var authentificatedUserIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimType.UserId)?.Value;

        if (authentificatedUserIdClaim is null)
        {
            return Unauthorized();
        }

        var userId = int.Parse(authentificatedUserIdClaim);

        var createdBusiness = await _mediator.Send(new CreateBusinessCommand(userId, businessToAdd), cancellationToken);

        return Ok(createdBusiness);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditBusiness([FromRoute] int id, [FromBody] EditBusinessDto businessToEdit, CancellationToken cancellationToken)
    {
        var updatedBusiness = await _mediator.Send(new EditBusinessCommand(id, businessToEdit), cancellationToken);

        return Ok(updatedBusiness);
    }

    //[HttpDelete("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //public async Task<IActionResult> DeleteBusiness(int id)
    //{
    //    await _mediator.Send(new DeleteBusinessCommand(id));

    //    return Ok();
    //}

    [HttpGet("{id}/logo")]
    public async Task<IActionResult> GetBusinessLogo([FromRoute] int id, CancellationToken cancellationToken)
    {
        return Ok();
    }
}