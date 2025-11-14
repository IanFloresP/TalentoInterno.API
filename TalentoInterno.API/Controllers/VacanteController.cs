using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VacanteController : ControllerBase
{
    private readonly IVacanteService _vacanteService;
    private readonly IColaboradorSkillService _colaboradorSkillService;
    private readonly IVacanteSkillReqService _vacanteSkillReqService;

    public VacanteController(IVacanteService vacanteService, IColaboradorSkillService colaboradorSkillService, IVacanteSkillReqService vacanteSkillReqService)
    {
        _vacanteService = vacanteService;
        _colaboradorSkillService = colaboradorSkillService;
        _vacanteSkillReqService = vacanteSkillReqService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vacantes = await _vacanteService.GetAllVacantesAsync();
        return Ok(vacantes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();
        return Ok(vacante);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VacanteDto vacanteDto)
    {
        await _vacanteService.CreateVacanteAsync(new Vacante
        {
            // Map properties from DTO to entity
            VacanteId = vacanteDto.VacanteId,
            Titulo = vacanteDto.Titulo,
            PerfilId = vacanteDto.PerfilId,
            CuentaId = vacanteDto.CuentaId,
            ProyectoId = vacanteDto.ProyectoId,
            FechaInicio = vacanteDto.FechaInicio,
            UrgenciaId = vacanteDto.UrgenciaId,
            Estado = vacanteDto.Estado,
            Descripcion = vacanteDto.Descripcion
        });
        return CreatedAtAction(nameof(GetById), new { id = vacanteDto.VacanteId }, vacanteDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] VacanteDto vacanteDto)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();

        // Map properties from DTO to entity
        vacante.Titulo = vacanteDto.Titulo;
        vacante.PerfilId = vacanteDto.PerfilId;
        vacante.CuentaId = vacanteDto.CuentaId;
        vacante.ProyectoId = vacanteDto.ProyectoId;
        vacante.FechaInicio = vacanteDto.FechaInicio;
        vacante.UrgenciaId = vacanteDto.UrgenciaId;
        vacante.Estado = vacanteDto.Estado;
        vacante.Descripcion = vacanteDto.Descripcion;

        await _vacanteService.UpdateVacanteAsync(vacante);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vacante = await _vacanteService.GetVacanteByIdAsync(id);
        if (vacante == null)
            return NotFound();

        await _vacanteService.DeleteVacanteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/skills")]
    public async Task<IActionResult> GetSkills(int id)
    {
        // Return the skill requirements for the vacante
        var reqs = await _vacanteSkillReqService.GetByVacanteIdAsync(id);
        var dto = reqs.Select(r => new VacanteSkillReqDto
        {
            VacanteId = r.VacanteId,
            SkillId = r.SkillId,
            NivelDeseado = r.NivelDeseado,
            Peso = r.Peso,
            Critico = r.Critico
        });
        return Ok(dto);
    }

    [HttpGet("{id}/matching")]
    public async Task<IActionResult> GetMatchingCandidates(int id)
    {
        // Compute matching candidates for a vacante
        var reqs = (await _vacanteSkillReqService.GetByVacanteIdAsync(id)).ToList();
        if (!reqs.Any()) return Ok(Enumerable.Empty<MatchingCandidateDto>());

        // compute available counts per skill
        var colaboradores = (await _colaboradorSkillService.GetColaboradoresWithSkillsAsync(null, null)).ToList();
        var availableCounts = new Dictionary<int, int>();
        foreach (var r in reqs)
        {
            var count = colaboradores.SelectMany(c => c.ColaboradorSkill)
                .Where(cs => cs.SkillId == r.SkillId && cs.NivelId >= r.NivelDeseado)
                .Select(cs => cs.ColaboradorId)
                .Distinct()
                .Count();
            availableCounts[r.SkillId] = count;
        }

        // determine if any critical skill has zero available (recruitment alert)
        var globalRecruitmentAlert = reqs.Any(r => (r.Critico ?? false) && availableCounts.GetValueOrDefault(r.SkillId) == 0);

        // total weight
        decimal totalWeight = reqs.Sum(r => r.Peso ?? 1m);
        if (totalWeight == 0) totalWeight = reqs.Count; // fallback

        var candidates = new List<MatchingCandidateDto>();

        foreach (var c in colaboradores)
        {
            decimal matchedWeight = 0m;
            var skillsDto = new List<ColaboradorSkillDto>();
            foreach (var r in reqs)
            {
                var cs = c.ColaboradorSkill.FirstOrDefault(x => x.SkillId == r.SkillId);
                var weight = r.Peso ?? 1m;
                if (cs != null)
                {
                    skillsDto.Add(new ColaboradorSkillDto
                    {
                        ColaboradorId = cs.ColaboradorId,
                        SkillId = cs.SkillId,
                        SkillNombre = cs.Skill?.Nombre,
                        NivelId = cs.NivelId,
                        NivelNombre = cs.Nivel?.Nombre,
                        AniosExp = cs.AniosExp
                    });
                    if (cs.NivelId >= r.NivelDeseado)
                    {
                        matchedWeight += weight;
                    }
                }
            }

            var score = totalWeight == 0 ? 0 : (matchedWeight / totalWeight) * 100m;

            candidates.Add(new MatchingCandidateDto
            {
                ColaboradorId = c.ColaboradorId,
                Nombre = $"{c.Nombres} {c.Apellidos}",
                MatchScore = Math.Round(score, 2),
                Skills = skillsDto,
                RecruitmentAlert = globalRecruitmentAlert
            });
        }

        var ordered = candidates.OrderByDescending(x => x.MatchScore).ToList();
        return Ok(ordered);
    }

    [HttpGet("{id}/ranking")]
    public async Task<IActionResult> GetRanking(int id)
    {
        // reuse matching logic and return ranked candidates (IDs and scores)
        var matchResult = (await GetMatchingCandidatesInternal(id));
        var ranking = matchResult.Select(m => new { m.ColaboradorId, m.Nombre, m.MatchScore }).OrderByDescending(x => x.MatchScore);
        return Ok(ranking);
    }

    // internal helper to reuse matching computation
    private async Task<List<MatchingCandidateDto>> GetMatchingCandidatesInternal(int id)
    {
        var reqs = (await _vacanteSkillReqService.GetByVacanteIdAsync(id)).ToList();
        if (!reqs.Any()) return new List<MatchingCandidateDto>();

        var colaboradores = (await _colaboradorSkillService.GetColaboradoresWithSkillsAsync(null, null)).ToList();
        var availableCounts = new Dictionary<int, int>();
        foreach (var r in reqs)
        {
            var count = colaboradores.SelectMany(c => c.ColaboradorSkill)
                .Where(cs => cs.SkillId == r.SkillId && cs.NivelId >= r.NivelDeseado)
                .Select(cs => cs.ColaboradorId)
                .Distinct()
                .Count();
            availableCounts[r.SkillId] = count;
        }

        var globalRecruitmentAlert = reqs.Any(r => (r.Critico ?? false) && availableCounts.GetValueOrDefault(r.SkillId) == 0);

        decimal totalWeight = reqs.Sum(r => r.Peso ?? 1m);
        if (totalWeight == 0) totalWeight = reqs.Count;

        var candidates = new List<MatchingCandidateDto>();

        foreach (var c in colaboradores)
        {
            decimal matchedWeight = 0m;
            var skillsDto = new List<ColaboradorSkillDto>();
            foreach (var r in reqs)
            {
                var cs = c.ColaboradorSkill.FirstOrDefault(x => x.SkillId == r.SkillId);
                var weight = r.Peso ?? 1m;
                if (cs != null)
                {
                    skillsDto.Add(new ColaboradorSkillDto
                    {
                        ColaboradorId = cs.ColaboradorId,
                        SkillId = cs.SkillId,
                        SkillNombre = cs.Skill?.Nombre,
                        NivelId = cs.NivelId,
                        NivelNombre = cs.Nivel?.Nombre,
                        AniosExp = cs.AniosExp
                    });
                    if (cs.NivelId >= r.NivelDeseado)
                    {
                        matchedWeight += weight;
                    }
                }
            }

            var score = totalWeight == 0 ? 0 : (matchedWeight / totalWeight) * 100m;

            candidates.Add(new MatchingCandidateDto
            {
                ColaboradorId = c.ColaboradorId,
                Nombre = $"{c.Nombres} {c.Apellidos}",
                MatchScore = Math.Round(score, 2),
                Skills = skillsDto,
                RecruitmentAlert = globalRecruitmentAlert
            });
        }

        return candidates.OrderByDescending(x => x.MatchScore).ToList();
    }
}