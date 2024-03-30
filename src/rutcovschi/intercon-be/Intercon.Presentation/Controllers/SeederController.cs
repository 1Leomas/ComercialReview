using Intercon.Infrastructure.Persistence.DataSeeder;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeederController(DataBaseSeeder seeder) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Seed()
    {
        await seeder.Seed();

        return Ok();
    }
}
