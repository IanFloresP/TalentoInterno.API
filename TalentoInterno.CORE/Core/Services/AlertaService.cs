using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class AlertaService : IAlertaService
{
    private readonly TalentoInternooContext _context;
    private readonly IMatchingService _matchingService;

    public AlertaService(TalentoInternooContext context, IMatchingService matchingService)
    {
        _context = context;
        _matchingService = matchingService;
    }

    public async Task<AlertasResponseDto> GenerarAlertasAsync(string? tipo = null, int? id = null, int? umbral = null)
    {
        var response = new AlertasResponseDto();

        // Definimos umbrales por defecto si no vienen en la URL
        int umbralMatch = (tipo == "vacante" && umbral.HasValue) ? umbral.Value : 50; // Default: 50% match
        int umbralEscasez = (tipo == "skillcritico" && umbral.HasValue) ? umbral.Value : 2; // Default: menos de 2 personas

        // --- 1. ANÁLISIS DE VACANTES (Si tipo es null o "vacante") ---
        if (string.IsNullOrEmpty(tipo) || tipo.ToLower() == "vacante")
        {
            var queryVacantes = _context.Vacante.Where(v => v.Estado == "Abierta");

            // Filtro por ID específico si se pide
            if (id.HasValue && tipo == "vacante")
            {
                queryVacantes = queryVacantes.Where(v => v.VacanteId == id.Value);
            }

            var vacantesAbiertas = await queryVacantes.ToListAsync();

            foreach (var vacante in vacantesAbiertas)
            {
                var candidatos = await _matchingService.GetRankedCandidatesAsync(vacante.VacanteId);

                // Verificamos si hay candidatos que superen el umbral de match
                var candidatosViables = candidatos.Count(c => c.PorcentajeMatch >= umbralMatch);

                if (candidatosViables == 0)
                {
                    response.Vacantes.Add(new AlertaDto
                    {
                        Tipo = "RIESGO_VACANTE",
                        Mensaje = $"La vacante '{vacante.Titulo}' no tiene candidatos con match > {umbralMatch}%.",
                        Criticidad = "Alta",
                        EntidadId = vacante.VacanteId,
                        EntidadNombre = vacante.Titulo,
                        ValorActual = 0, // 0 candidatos
                        Detalles = $"Se encontraron {candidatos.Count()} candidatos totales, pero ninguno supera el umbral de match.",
                        Sugerencias = new List<string>
                        {
                            "Revisar los requisitos de la vacante para hacerlos más accesibles.",
                            "Ampliar la búsqueda a otras áreas o niveles de experiencia.",
                            "Promover la vacante en canales adicionales."
                        }
                    });
                }
            }
        }

        // --- 2. ANÁLISIS DE SKILLS CRÍTICOS (Si tipo es null o "skillcritico") ---
        if (string.IsNullOrEmpty(tipo) || tipo.ToLower() == "skillcritico")
        {
            var querySkills = _context.VacanteSkillReq
                .Where(r => r.Critico == true)
                .Select(r => r.SkillId)
                .Distinct();

            // Filtro por Skill ID específico
            if (id.HasValue && tipo == "skillcritico")
            {
                querySkills = querySkills.Where(sId => sId == id.Value);
            }

            var skillsCriticasIds = await querySkills.ToListAsync();

            foreach (var skillId in skillsCriticasIds)
            {
                var conteo = await _context.ColaboradorSkill.CountAsync(cs => cs.SkillId == skillId);

                // Si la cantidad de gente es menor al umbral (ej: < 2)
                if (conteo < umbralEscasez)
                {
                    var skillNombre = await _context.Skill
                        .Where(s => s.SkillId == skillId)
                        .Select(s => s.Nombre)
                        .FirstOrDefaultAsync();

                    response.Skills.Add(new AlertaDto
                    {
                        Tipo = "ESCASEZ_SKILL",
                        Mensaje = $"La habilidad crítica '{skillNombre}' solo la tienen {conteo} personas (Umbral: {umbralEscasez}).",
                        Criticidad = "Critica",
                        EntidadId = skillId,
                        EntidadNombre = skillNombre,
                        ValorActual = conteo,
                        Detalles = $"Esta habilidad es requerida en varias vacantes críticas.",
                        Sugerencias = new List<string>
                        {
                            "Implementar programas de capacitación para esta habilidad.",
                            "Fomentar la rotación interna para desarrollar esta skill en más colaboradores.",
                            "Considerar candidatos externos con esta habilidad para futuras contrataciones."
                        }
                    });
                }
            }
        }

        return response;
    }
}