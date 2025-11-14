using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Entities;

namespace TalentoInterno.CORE.Core.Services;

public class MatchingService : IMatchingService
{
    private readonly IVacanteSkillReqRepository _vacanteReqRepo;
    private readonly IColaboradorRepository _colaboradorRepo;
    // NOTA: No necesitamos 'IColaboradorSkillRepository' aquí si 'IColaboradorRepository.GetAllAsync()'
    // ya incluye las skills del colaborador, lo cual es más eficiente.

    public MatchingService(
        IVacanteSkillReqRepository vacanteReqRepo,
        IColaboradorRepository colaboradorRepo)
    {
        _vacanteReqRepo = vacanteReqRepo;
        _colaboradorRepo = colaboradorRepo;
    }

    public async Task<IEnumerable<MatchResultDTO>> GetRankedCandidatesAsync(int vacanteId)
    {
        // 1. Obtener los requisitos de la vacante (con Skill y Nivel)
        var skillsRequeridas = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);

        // 2. Obtener todos los colaboradores (con sus Skills y Nivel)
        //    (Tu ColaboradorRepository.GetAllAsync() DEBE hacer .Include(c => c.ColaboradorSkill).ThenInclude(cs => cs.Nivel))
        var colaboradores = await _colaboradorRepo.GetAllAsync();

        var ranking = new List<MatchResultDTO>();

        foreach (var colaborador in colaboradores)
        {
            var skillsColaborador = colaborador.ColaboradorSkill;

            var resultado = new MatchResultDTO
            {
                ColaboradorId = colaborador.ColaboradorId,
                VacanteId = vacanteId
            };

            // Estas variables calculan el match para CADA colaborador
            decimal maxPuntosPosibles = 0;
            decimal totalPuntosObtenidos = 0;
            bool falloCritica = false;

            // Si la vacante no pide skills, todos tienen 100%
            if (!skillsRequeridas.Any())
            {
                resultado.PorcentajeMatch = 100;
                ranking.Add(resultado);
                continue; // Pasa al siguiente colaborador
            }

            // --- INICIO DE LA LÓGICA DE CÁLCULO ---
            foreach (var req in skillsRequeridas)
            {
                // 1. Obtiene el peso de la skill (ej: 1.0, 0.5)
                decimal pesoSkill = req.Peso ?? 0;
                maxPuntosPosibles += pesoSkill;

                // 2. Busca la skill en el colaborador
                var colabSkill = skillsColaborador.FirstOrDefault(cs => cs.SkillId == req.SkillId);

                // 3. Comprueba si cumple el nivel (ej: Nivel 3 >= Requerido 3)
                bool cumpleNivel = (colabSkill != null && colabSkill.NivelId >= req.NivelDeseado);

                // 4. Crea el objeto de detalle (para 'skillsQueCumple' o 'skillsFaltantes')
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

                // 5. Asigna puntos y llena las listas
                if (cumpleNivel)
                {
                    totalPuntosObtenidos += pesoSkill; // Suma los puntos ganados
                    resultado.SkillsQueCumple.Add(detalle);
                }
                else
                {
                    resultado.SkillsFaltantes.Add(detalle);

                    // 6. Comprueba si era una skill crítica
                    if (req.Critico == true)
                    {
                        falloCritica = true;
                    }
                }
            }
            // --- FIN DE LA LÓGICA DE CÁLCULO ---

            // 7. Calcula el porcentaje final
            if (falloCritica)
            {
                resultado.PorcentajeMatch = 0; // Descalificado por fallar skill crítica
            }
            else if (maxPuntosPosibles > 0)
            {
                resultado.PorcentajeMatch = (double)Math.Round((totalPuntosObtenidos / maxPuntosPosibles) * 100, 2); // Cálculo ponderado
            }
            else
            {
                resultado.PorcentajeMatch = 100; // Si no había pesos, 100%
            }

            ranking.Add(resultado);
        }

        // 8. Devuelve la lista ordenada (los mejores primero)
        return ranking.OrderByDescending(r => r.PorcentajeMatch);
    }
}