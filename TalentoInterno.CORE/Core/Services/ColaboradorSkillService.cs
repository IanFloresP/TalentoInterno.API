using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

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

    public async Task<IEnumerable<object>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId)
    {
        // Simple algorithm (placeholder for full algorithm):
        // For each skill requirement of the vacante,
        //  - count number of colaboradores in area (or all) with nivel >= nivelDeseado
        //  - gap = requiredCount (1) - availableCount
        //  - flag recruitment if skill is critical and availableCount == 0
        // Full algorithm will consider pesos, experiencia y ranking de candidatos.

        var reqs = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);
        var result = new List<object>();

        foreach (var r in reqs)
        {
            // count colaboradores with the skill and level >= desired
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
