using Intercon.Application.BusinessesManagement.CreateBusiness;
using Intercon.Application.BusinessesManagement.EditBusiness;
using Intercon.Application.BusinessesManagement.GetBusiness;
using Intercon.Application.BusinessesManagement.GetBusinesses;
using Intercon.Application.BusinessesManagement.GetPaginatedBusinesses;
using Intercon.Application.CustomExceptions;
using Intercon.Application.DataTransferObjects;
using Intercon.Application.DataTransferObjects.Business;
using Intercon.Application.FilesManagement.UploadFile;
using Intercon.Domain.Pagination;
using Intercon.Presentation.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/businesses")]
[ApiController]
public class BusinessController(IMediator mediator) : BaseController
{
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBusiness([FromRoute] int id, CancellationToken cancellationToken)
    {
        var business = await mediator.Send(new GetBusinessQuery(id), cancellationToken);

        return business == null ? NotFound() : Ok(business);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<BusinessDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBusinesses(CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetAllBusinessesQuery(), cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<BusinessDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedBusinesses([FromQuery] BusinessParameters parameters, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(new GetPaginatedBusinessesQuery(parameters), cancellationToken));
    }

    [Authorize(Roles = "2")]
    [HttpPost]
    [ProducesResponseType(typeof(BusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateBusiness([FromForm] CreateBusinessDto businessToAdd, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        var createdBusiness = await mediator.Send(
            new CreateBusinessCommand(currentUserId, businessToAdd), 
            cancellationToken);

        return Ok(createdBusiness);
    }

    [Authorize]
    [HttpPut("{businessId}/edit")]
    [ProducesResponseType(typeof(EditBusinessDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditBusiness([FromRoute] int businessId, [FromForm] EditBusinessRequest businessToEdit, CancellationToken cancellationToken)
    {
        var currentUserId = HttpContext.User.GetUserId();

        int? newLogoId = null!;
        if (businessToEdit.Logo is not null)
        {
            var logoFileData = await mediator.Send(new UploadFileCommand(businessToEdit.Logo), cancellationToken);

            if (logoFileData is null)
                throw new InvalidOperationException("Can not upload logo");

            newLogoId = logoFileData.Id;
        }
        
        var updatedBusiness = await mediator.Send(new EditBusinessCommand(currentUserId, businessId, businessToEdit, newLogoId), cancellationToken);

        return Ok(updatedBusiness);
    }
}