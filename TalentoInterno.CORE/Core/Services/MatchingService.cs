using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace TalentoInterno.CORE.Core.Services;

public class MatchingService : IMatchingService
{
    private readonly IVacanteSkillReqRepository _vacanteReqRepo;
    private readonly IColaboradorRepository _colaboradorRepo;

    public MatchingService(
        IVacanteSkillReqRepository vacanteReqRepo,
        IColaboradorRepository colaboradorRepo)
    {
        _vacanteReqRepo = vacanteReqRepo;
        _colaboradorRepo = colaboradorRepo;
    }

    public async Task<IEnumerable<MatchResultDTO>> GetRankedCandidatesAsync(int vacanteId)
    {
        var skillsRequeridas = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);
        var todosLosColaboradores = await _colaboradorRepo.GetAllAsync();

        // --- NUEVO FILTRO ---
        // Excluimos a los Administradores (ID 3) del proceso de selección.
        // También podríamos excluir a los inactivos si no lo hace el repositorio.
        var candidatos = todosLosColaboradores
            .Where(c => c.RolId != 3 && c.Activo == true)
            .ToList();
        // --------------------

        var ranking = new List<MatchResultDTO>();

        foreach (var colaborador in candidatos) // Usamos la lista filtrada
        {
            var skillsColaborador = colaborador.ColaboradorSkill;
            var resultado = new MatchResultDTO
            {
                ColaboradorId = colaborador.ColaboradorId,
                Nombre = $"{colaborador.Nombres} {colaborador.Apellidos}",
                VacanteId = vacanteId
            };

            decimal maxPuntosPosibles = 0;
            decimal totalPuntosObtenidos = 0;
            bool falloCritica = false;

            if (!skillsRequeridas.Any())
            {
                resultado.PorcentajeMatch = 100;
                ranking.Add(resultado);
                continue;
            }

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

            if (falloCritica) resultado.PorcentajeMatch = 0;
            else if (maxPuntosPosibles > 0) resultado.PorcentajeMatch = (double)Math.Round((totalPuntosObtenidos / maxPuntosPosibles) * 100, 2);
            else resultado.PorcentajeMatch = 100;

            ranking.Add(resultado);
        }

        return ranking.OrderByDescending(r => r.PorcentajeMatch);
    }
}