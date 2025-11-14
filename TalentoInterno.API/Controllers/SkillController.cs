using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Para usar .Select()

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

    // --- HU-11, HU-12: Listar todos los habilidades ---
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _skillService.GetAllSkillsAsync();

        // Mapeamos la lista de Entidades a una lista de DTOs
        var skillsDto = skills.Select(s => new SkillDto
        {
            SkillId = s.SkillId,
            Nombre = s.Nombre,
            TipoSkillId = s.TipoSkillId,
            TipoSkillNombre = s.TipoSkill?.Nombre,
            Critico = s.Critico
        });

        return Ok(skillsDto);
    }

    // --- HU-11: Ver detallado de una habilidad ---
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);

        if (skill == null)
        {
            return NotFound($"No se encontró la skill con ID {id}.");
        }

        // Mapeamos la Entidad a un DTO
        var skillDto = new SkillDto
        {
            SkillId = skill.SkillId,
            Nombre = skill.Nombre,
            TipoSkillId = skill.TipoSkillId,
            TipoSkillNombre = skill.TipoSkill?.Nombre,
            Critico = skill.Critico
        };

        return Ok(skillDto);
    }

    // --- HU-02: Registrador habilidad nuevo ---
    [HttpPost]
    public async Task<IActionResult> CreateSkill([FromBody] SkillCreateDTO dto)
    {
        // 1. Llamamos al servicio con el DTO de creación
        var newSkill = await _skillService.CreateSkillAsync(dto);

        // 2. Mapeamos la nueva entidad (con su ID) al DTO de respuesta
        var skillDto = new SkillDto
        {
            SkillId = newSkill.SkillId,
            Nombre = newSkill.Nombre,
            TipoSkillId = newSkill.TipoSkillId,
            TipoSkillNombre = newSkill.TipoSkill?.Nombre,
            Critico = newSkill.Critico
        };

        // 3. Devolvemos 201 CreatedAtAction (la mejor práctica)
        return CreatedAtAction(nameof(GetById), new { id = newSkill.SkillId }, skillDto);
    }
}