using Intercon.Infrastructure.Persistence.DataSeeder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Intercon.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeederController(DataBaseSeeder seeder) : ControllerBase
{
    private readonly DataBaseSeeder _seeder = seeder;

    [HttpGet]
    public async Task<IActionResult> Seed()
    {
        await seeder.Seed();

        return Ok();
    }
}
