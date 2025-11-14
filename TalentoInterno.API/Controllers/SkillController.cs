using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _skillService.GetAllSkillsAsync();
        var dto = skills.Select(s => new { s.SkillId, s.Nombre, s.Critico, s.TipoSkillId });
        return Ok(dto);
    }
}