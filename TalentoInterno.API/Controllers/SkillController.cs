using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin, RRHH")]
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

        var skillsDto = skills.Select(s => new SkillDto
        {
            SkillId = s.SkillId,
            Nombre = s.Nombre,

            TipoSkillId = s.TipoSkillId, // <--- ¡FALTABA ESTO!

            Critico = s.Critico,
            TipoSkillNombre = s.TipoSkill?.Nombre
        });

        return Ok(skillsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);

        if (skill == null)
        {
            return NotFound($"No se encontró la skill con ID {id}.");
        }

        var skillDto = new SkillDto
        {
            SkillId = skill.SkillId,
            Nombre = skill.Nombre,

            TipoSkillId = skill.TipoSkillId, // <--- ¡FALTABA ESTO!

            Critico = skill.Critico,
            TipoSkillNombre = skill.TipoSkill?.Nombre
        };

        return Ok(skillDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkill([FromBody] SkillCreateDTO dto)
    {
        var newSkill = await _skillService.CreateSkillAsync(dto);

        var skillDto = new SkillDto
        {
            SkillId = newSkill.SkillId,
            Nombre = newSkill.Nombre,

            TipoSkillId = newSkill.TipoSkillId, // <--- ¡FALTABA ESTO!
            TipoSkillNombre = newSkill.TipoSkill?.Nombre,

            Critico = newSkill.Critico
        };

        return CreatedAtAction(nameof(GetById), new { id = newSkill.SkillId }, skillDto);
    }
}