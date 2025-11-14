using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using System.Linq;
using TalentoInterno.CORE.Infrastructure.Repositories;

namespace TalentoInterno.CORE.Core.Services;

public class ColaboradorSkillService : IColaboradorSkillService
{
    private readonly IColaboradorSkillRepository _repository;
    private readonly IVacanteSkillReqRepository _vacanteReqRepo;

    public ColaboradorSkillService(IColaboradorSkillRepository repository, IVacanteSkillReqRepository vacanteReqRepo)
    {
        _repository = repository;
        _vacanteReqRepo = vacanteReqRepo;
    }

    public async Task<IEnumerable<ColaboradorSkill>> GetSkillsByColaboradorAsync(int colaboradorId)
    {
        return await _repository.GetByColaboradorIdAsync(colaboradorId);
    }

    public async Task<IEnumerable<Colaborador>> GetColaboradoresWithSkillsAsync(int? areaId, int? rolId)
    {
        return await _repository.GetColaboradoresWithSkillsAsync(areaId, rolId);
    }

    public async Task RegisterSkillsAsync(int colaboradorId, IEnumerable<ColaboradorSkillCreateDTO> skillsDTO)
    {
        var nuevasSkills = skillsDTO.Select(dto => new ColaboradorSkill
        {
            ColaboradorId = colaboradorId,
            SkillId = dto.SkillId,
            NivelId = dto.NivelId,
            AniosExp = dto.AniosExp
        });

        await _repository.AddSkillsAsync(nuevasSkills);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateSkillAsync(int colaboradorId, int skillId, ColaboradorSkillUpdateDTO skillDTO)
    {
        var skillExistente = await _repository.GetSingleAsync(colaboradorId, skillId);
        if (skillExistente == null)
        {
            throw new KeyNotFoundException("La skill para este colaborador no fue encontrada.");
        }
        skillExistente.NivelId = skillDTO.NivelId;
        skillExistente.AniosExp = skillDTO.AniosExp;

        await _repository.UpdateSkillAsync(skillExistente);
        await _repository.SaveChangesAsync();
    }

    public async Task<MatchResultDTO> GetMatchDetailsAsync(int colaboradorId, int vacanteId)
    {
        var skillsColaborador = await _repository.GetByColaboradorIdAsync(colaboradorId);
        var skillsRequeridas = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);

        var resultado = new MatchResultDTO
        {
            ColaboradorId = colaboradorId,
            VacanteId = vacanteId
        };

        if (!skillsRequeridas.Any())
        {
            resultado.PorcentajeMatch = 100;
            return resultado;
        }

        decimal maxPuntosPosibles = 0;
        decimal totalPuntosObtenidos = 0;
        bool falloCritica = false;

        foreach (var req in skillsRequeridas)
        {
            decimal pesoSkill = req.Peso ?? 0;
            maxPuntosPosibles += pesoSkill;

            var colabSkill = skillsColaborador.FirstOrDefault(cs => cs.SkillId == req.SkillId);
            bool cumpleNivel = (colabSkill != null && colabSkill.NivelId >= req.NivelDeseado);

            var detalle = new SkillMatchDetalleDTO
            {
                SkillId = req.SkillId,
                Nombre = req.Skill.Nombre,
                NivelRequeridoId = req.NivelDeseado,
                NivelRequeridoNombre = req.NivelDeseadoNavigation?.Nombre,
                NivelColaboradorId = colabSkill?.NivelId,
                NivelColaboradorNombre = colabSkill?.Nivel?.Nombre,
                CumpleNivel = cumpleNivel
            };

            if (cumpleNivel)
            {
                totalPuntosObtenidos += pesoSkill;
                resultado.SkillsQueCumple.Add(detalle);
            }
            else
            {
                resultado.SkillsFaltantes.Add(detalle);
                if (req.Critico == true)
                {
                    falloCritica = true;
                }
            }
        }

        if (falloCritica) { resultado.PorcentajeMatch = 0; }
        else if (maxPuntosPosibles > 0) { resultado.PorcentajeMatch = (double)Math.Round((totalPuntosObtenidos / maxPuntosPosibles) * 100, 2); }
        else { resultado.PorcentajeMatch = 100; }

        return resultado;
    }

    public async Task<IEnumerable<object>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId)
    {
        var reqs = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);
        var result = new List<object>();

        foreach (var r in reqs)
        {
            var colaboradores = await _repository.GetColaboradoresWithSkillsAsync(areaId, null);
            var available = colaboradores.SelectMany(c => c.ColaboradorSkill)
                .Where(cs => cs.SkillId == r.SkillId && cs.NivelId >= r.NivelDeseado)
                .Select(cs => cs.ColaboradorId)
                .Distinct()
                .Count();

            var gap = Math.Max(0, 1 - available);

            result.Add(new
            {
                SkillId = r.SkillId,
                SkillNombre = r.Skill.Nombre,
                NivelDeseado = r.NivelDeseado,
                AvailableCount = available,
                Gap = gap,
                Critico = r.Critico ?? false,
                RecruitmentAlert = (r.Critico ?? false) && available == 0
            });
        }
        return result;
    }
}