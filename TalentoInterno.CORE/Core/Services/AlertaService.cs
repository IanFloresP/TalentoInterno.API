using Microsoft.Extensions.Logging;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Services;

public class AlertaService : IAlertaService
{
    private readonly ILogger<AlertaService> _logger;

    public AlertaService(ILogger<AlertaService> logger)
    {
        _logger = logger;
    }

    public AlertaDTO AlertarVacante(int vacanteId, int umbral)
    {
        _logger.LogWarning("Vacante {VacanteId} sin candidatos internos o score bajo (umbral {Umbral})", vacanteId, umbral);

        return new AlertaDTO
        {
            Tipo = "Vacante",
            Severidad = "Alta",
            Mensaje = $"La vacante {vacanteId} no tiene candidatos internos compatibles.",
            Detalles = $"Umbral mínimo requerido: {umbral}",
            Sugerencias = new List<string> { "Considerar reclutamiento externo", "Revisar requisitos de la vacante" }
        };
    }

    public AlertaDTO AlertarSkillCritico(int skillId, int umbral)
    {
        _logger.LogWarning("Skill crítico {SkillId} sin cobertura suficiente (umbral {Umbral})", skillId, umbral);

        return new AlertaDTO
        {
            Tipo = "SkillCritico",
            Severidad = "Alta",
            Mensaje = $"El skill crítico {skillId} no tiene cobertura suficiente.",
            Detalles = $"Umbral mínimo requerido: {umbral}",
            Sugerencias = new List<string> { "Capacitar colaboradores existentes", "Contratar perfiles externos especializados" }
        };
    }
}