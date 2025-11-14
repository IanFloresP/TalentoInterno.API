using Microsoft.AspNetCore.Mvc;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        // Implement logic for HU-11, HU-12
        return Ok();
    }
}