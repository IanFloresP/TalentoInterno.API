using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;

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

        // Filtramos administradores para no ensuciar el ranking
        var candidatos = await _colaboradorRepo.GetCandidatosCompletosAsync();

        var ranking = new List<MatchResultDTO>();

        foreach (var colaborador in candidatos)
        {
            var skillsColaborador = colaborador.ColaboradorSkill;
            var resultado = new MatchResultDTO
            {
                ColaboradorId = colaborador.ColaboradorId,
                Nombre = $"{colaborador.Nombres} {colaborador.Apellidos}",
                VacanteId = vacanteId,
                Email = colaborador.Email,
                AreaNombre = colaborador.Area?.Nombre ?? "Sin Área",
                DepartamentoNombre = colaborador.Departamento?.Nombre ?? "Sin Depto",
                RolNombre = colaborador.Rol?.Nombre ?? "Sin Rol"
            };

            decimal maxPuntosPosibles = 0;
            decimal totalPuntosObtenidos = 0;
            bool falloCritica = false;

            // Caso borde: Si la vacante no tiene skills, todos tienen 100%
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

                // Datos para el reporte (Excel/PDF)
                int nivelActual = colabSkill?.NivelId ?? 0;
                int nivelRequerido = req.NivelDeseado;

                var detalle = new SkillMatchDetalleDTO
                {
                    SkillId = req.SkillId,
                    Nombre = req.Skill.Nombre,
                    NivelRequeridoId = nivelRequerido,
                    NivelRequeridoNombre = req.NivelDeseadoNavigation?.Nombre,
                    NivelColaboradorId = colabSkill?.NivelId,
                    NivelColaboradorNombre = colabSkill?.Nivel?.Nombre,
                    CumpleNivel = (nivelActual >= nivelRequerido)
                };

                // --- LÓGICA DE PUNTOS PARCIALES ---

                if (colabSkill != null) // TIENE LA HABILIDAD
                {
                    if (nivelActual >= nivelRequerido)
                    {
                        // CASO 1: CUMPLE O SUPERA -> PUNTOS COMPLETOS
                        totalPuntosObtenidos += pesoSkill;
                        resultado.SkillsQueCumple.Add(detalle);
                    }
                    else
                    {
                        // CASO 2: TIENE MENOS NIVEL -> PUNTOS PROPORCIONALES
                        // Fórmula: (NivelQueTengo / NivelQuePiden) * Peso
                        // Ejemplo: Tengo 1, Piden 2, Peso 5 -> (1/2)*5 = 2.5 puntos ganados.
                        if (nivelRequerido > 0)
                        {
                            decimal factor = (decimal)nivelActual / (decimal)nivelRequerido;
                            totalPuntosObtenidos += (pesoSkill * factor);
                        }

                        // Aunque sume puntos, técnicamente es una "Brecha" de nivel, así que va a Faltantes
                        // para que en el Excel salga la barrita roja/amarilla de lo que falta.
                        resultado.SkillsFaltantes.Add(detalle);

                        // NOTA: Aquí NO activamos 'falloCritica'. 
                        // Permitimos que sume puntos parciales aunque sea crítica, 
                        // para ver variedad en el ranking.
                    }
                }
                else // NO TIENE LA HABILIDAD DEL TODO
                {
                    // CASO 3: NO SABE NADA (0 Puntos)
                    resultado.SkillsFaltantes.Add(detalle);

                    // Aquí SÍ aplicamos el castigo si es crítica
                    if (req.Critico == true)
                    {
                        falloCritica = true; // Kill Switch: Si no la tienes del todo, adiós.
                    }
                }
            }

            // CÁLCULO FINAL
            if (falloCritica)
            {
                resultado.PorcentajeMatch = 0;
            }
            else if (maxPuntosPosibles > 0)
            {
                // Redondeamos a 2 decimales
                resultado.PorcentajeMatch = (double)Math.Round((totalPuntosObtenidos / maxPuntosPosibles) * 100, 2);
            }
            else
            {
                resultado.PorcentajeMatch = 100;
            }

            ranking.Add(resultado);
        }

        return ranking.OrderByDescending(r => r.PorcentajeMatch);
    }
}