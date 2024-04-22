using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult InternalServerError()
    {
        return new StatusCodeResult(500);
    }
}