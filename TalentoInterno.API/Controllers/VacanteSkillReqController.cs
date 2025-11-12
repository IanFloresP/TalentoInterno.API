using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs; // Asegúrate de que este using esté presente y que VacanteSkillReqDto exista en este espacio de nombres

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanteSkillReqController : ControllerBase
{
    private readonly IVacanteSkillReqService _vacanteSkillReqService;

    public VacanteSkillReqController(IVacanteSkillReqService vacanteSkillReqService)
    {
        _vacanteSkillReqService = vacanteSkillReqService;
    }

    [HttpGet("{vacanteId}/skills")]
    public IActionResult GetSkills(int vacanteId)
    {
        // Implement logic for HU-09
        return Ok();
    }

    [HttpPost("{vacanteId}/skills")]
    public IActionResult AddSkill(int vacanteId, [FromBody] VacanteSkillReqDto skillDto)
    {
        // Implement logic for HU-09
        return Ok();
    }

    [HttpPut("{vacanteId}/skills/{skillId}")]
    public IActionResult UpdateSkill(int vacanteId, int skillId, [FromBody] VacanteSkillReqDto skillDto)
    {
        // Implement logic for HU-09
        return Ok();
    }
}