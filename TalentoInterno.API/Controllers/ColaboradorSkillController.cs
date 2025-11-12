using Microsoft.AspNetCore.Mvc;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColaboradorSkillController : ControllerBase
{
    [HttpGet("{colaboradorId}/skills")]
    public IActionResult GetSkills(int colaboradorId)
    {
        // Implement logic for HU-09, HU-11
        return Ok();
    }

    [HttpGet("{colaboradorId}/match/{vacanteId}")]
    public IActionResult GetMatch(int colaboradorId, int vacanteId)
    {
        // Implement logic for HU-09
        return Ok();
    }
}