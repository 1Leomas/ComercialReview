using Intercon.Application.BusinessesManagement.CreateBusiness;
using Intercon.Application.BusinessesManagement.GetBusiness;
using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects.Business;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/business")]
[ApiController]
public class BusinessController(IMediator mediator) : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBusiness(int id)
    {
        var business = await mediator.Send(new GetBusinessQuery(id));

        if (business == null)
        {
            return NotFound();
        }

        return Ok(business);
    }

    //[HttpGet]
    //[ProducesResponseType(typeof(IEnumerable<BusinessDetailsDto>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetBusinesses()
    //{
    //    return Ok(await mediator.Send(new GetBusinessesQuery()));
    //}

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessDto businessToAdd)
    {
        await mediator.Send(new CreateBusinessCommand(businessToAdd));

        return Ok();
    }

    //[HttpPut]
    //[ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    //public async Task<IActionResult> EditBusiness([FromQuery] int id, [FromBody] EditBusinessDto businessToEdit)
    //{
    //    await mediator.Send(new EditBusinessCommand(id, businessToEdit));

    //    return Ok();
    //}

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteBusiness(int id)
    {
        //await mediator.Send(new DeleteBusinessCommand(id));

        return Ok();
    }

    [HttpGet]
    [Route("logo")]
    public async Task<IActionResult> GetBusinessLogo([FromQuery(Name = "logo_id")] int businessId, CancellationToken cancellationToken)
    {
        return Ok();
    }
}