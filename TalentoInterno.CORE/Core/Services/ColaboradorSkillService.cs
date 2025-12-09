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
    private readonly IColaboradorRepository _colaboradorRepo; // Necesitamos traer a los colaboradores

    // Inyectamos también el repositorio de colaboradores
    public ColaboradorSkillService(
        IColaboradorSkillRepository repository,
        IVacanteSkillReqRepository vacanteReqRepo,
        IColaboradorRepository colaboradorRepo)
    {
        _repository = repository;
        _vacanteReqRepo = vacanteReqRepo;
        _colaboradorRepo = colaboradorRepo;
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

    public async Task<IEnumerable<BrechaSkillDto>> GetSkillGapsForVacanteAsync(int vacanteId, int? areaId = null)
    {
        // 1. Obtener requisitos de la vacante
        var requisitos = await _vacanteReqRepo.GetByVacanteIdAsync(vacanteId);

        // 2. Obtener colaboradores
        var todosLosColaboradores = await _colaboradorRepo.GetAllAsync();

        // 3. FILTRAR: Excluir Admins (3), Inactivos y filtrar por ÁREA si se pide
        var candidatos = todosLosColaboradores
            .Where(c => c.RolId != 3 && c.Activo == true);

        // --- APLICAMOS EL FILTRO DE ÁREA AQUÍ ---
        if (areaId.HasValue)
        {
            candidatos = candidatos.Where(c => c.AreaId == areaId.Value);
        }

        var listaBrechas = new List<BrechaSkillDto>();

        foreach (var colab in candidatos)
        {
            foreach (var req in requisitos)
            {
                var skillColab = colab.ColaboradorSkill.FirstOrDefault(s => s.SkillId == req.SkillId);

                int nivelReq = req.NivelDeseado;
                int nivelAct = skillColab?.NivelId ?? 0;

                double brecha = 0;
                if (nivelReq > nivelAct)
                {
                    var ratio = (double)(nivelReq - nivelAct) / 3.0;
                    brecha = Math.Round(ratio * 100, 2);

                }

                listaBrechas.Add(new BrechaSkillDto
                {
                    ColaboradorId = colab.ColaboradorId,
                    NombreColaborador = $"{colab.Nombres} {colab.Apellidos}",
                    SkillNombre = req.Skill.Nombre,
                    NivelRequerido = req.NivelDeseadoNavigation?.Nombre ?? "N/A",
                    NivelRequeridoId = nivelReq,
                    NivelActual = skillColab?.Nivel?.Nombre ?? "No tiene",
                    NivelActualId = nivelAct,
                    BrechaPorcentaje = brecha
                });
            }
        }

        return listaBrechas.OrderByDescending(b => b.BrechaPorcentaje).ThenBy(b => b.NombreColaborador);
    }
}