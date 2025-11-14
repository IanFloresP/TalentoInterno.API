using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs; // Importar DTOs
using System.Linq; // Importar LINQ

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

    // --- MÉTODOS AÑADIDOS Y MODIFICADOS ---

    // Implementación de POST
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

    // Implementación de PUT
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

    // Implementación de GET /match/
    public async Task<MatchResultDTO> GetMatchDetailsAsync(int colaboradorId, int vacanteId)
    {
        // 1. Obtener skills del colaborador (con Includes)
        var skillsColaborador = await _repository.GetByColaboradorIdAsync(colaboradorId);

        // 2. Obtener skills requeridas por la vacante (con Includes)
        // ¡Asegúrate que GetByVacanteIdAsync incluya .Skill y .NivelDeseadoNavigation!
        var skillsRequeridas = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);

        var resultado = new MatchResultDTO
        {
            ColaboradorId = colaboradorId,
            VacanteId = vacanteId
        };

        if (!skillsRequeridas.Any())
        {
            resultado.PorcentajeMatch = 100; // Si no se pide nada, 100% match
            return resultado;
        }

        // --- LÓGICA DE PESOS Y CRÍTICO ---
        decimal maxPuntosPosibles = 0;
        decimal totalPuntosObtenidos = 0;
        bool falloCritica = false;
        // ------------------------------------

        foreach (var req in skillsRequeridas)
        {
            // 1. Sumar el peso de esta skill al máximo posible
            decimal pesoSkill = req.Peso ?? 0;
            maxPuntosPosibles += pesoSkill;

            var colabSkill = skillsColaborador.FirstOrDefault(cs => cs.SkillId == req.SkillId);

            // 2. Comprobar si cumple el nivel
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
                // 3. Si cumple, sumar los puntos al total del colaborador
                totalPuntosObtenidos += pesoSkill;
                resultado.SkillsQueCumple.Add(detalle);
            }
            else
            {
                resultado.SkillsFaltantes.Add(detalle);

                // 4. VALIDACIÓN CRÍTICA
                // Si el requisito era crítico y no lo cumplió...
                if (req.Critico == true)
                {
                    falloCritica = true;
                }
            }
        }

        // --- CÁLCULO FINAL MODIFICADO ---
        if (falloCritica)
        {
            // 5. Si falló CUALQUIER skill crítica, el match es 0%
            resultado.PorcentajeMatch = 0;
        }
        else if (maxPuntosPosibles > 0)
        {
            // 6. Si no falló, calcula el porcentaje por peso
            resultado.PorcentajeMatch = (double)Math.Round((totalPuntosObtenidos / maxPuntosPosibles) * 100, 2);
        }
        else
        {
            resultado.PorcentajeMatch = 100; // Si ninguna skill tenía peso, 100% match
        }

        return resultado;
    }


    // Tu método GetSkillGapsForVacanteAsync
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

            var gap = Math.Max(0, 1 - available); // Asumiendo que 1 es el count requerido

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