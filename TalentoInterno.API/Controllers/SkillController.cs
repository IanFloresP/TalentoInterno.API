using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin, RRHH")]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;
    private readonly TalentoInternooContext _context;
    public SkillController(ISkillService skillService, TalentoInternooContext context)
    {
        _skillService = skillService;
        _context = context;
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
    [HttpGet("tiposkill")]
    public async Task<IActionResult> GetTiposSkill()
    {
        var data = await _context.TipoSkill
            .Select(t => new TipoSkillDto
            {
                TipoSkillId = t.TipoSkillId,
                Nombre = t.Nombre
            })
            .ToListAsync();
        return Ok(data);
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