using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces;

public interface IAlertaService
{
    AlertaDTO AlertarVacante(int vacanteId, int umbral);
    AlertaDTO AlertarSkillCritico(int skillId, int umbral);
}