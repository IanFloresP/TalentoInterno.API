using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TalentoInterno.CORE.Core.Services;

public class PostulacionService : IPostulacionService
{
    private readonly IPostulacionRepository _repository;
    private readonly IMatchingService _matchingService;
    private readonly IVacanteRepository _vacanteRepo;
    private readonly TalentoInternooContext _context;

    public PostulacionService(
        IPostulacionRepository repository,
        IMatchingService matchingService,
        IVacanteRepository vacanteRepo,
        TalentoInternooContext context)
    {
        _repository = repository;
        _matchingService = matchingService;
        _vacanteRepo = vacanteRepo;
        _context = context;
    }

    public async Task<IEnumerable<PostulacionDto>> CrearMasivoAsync(CrearPostulacionMasivaDto dto)
    {
        var vacante = await _context.Vacante.FindAsync(dto.VacanteId);
        if (vacante == null)
            throw new KeyNotFoundException($"No existe la vacante con id {dto.VacanteId}.");

        var ids = dto.ColaboradorIds.Distinct().ToList();

        var colaboradoresExistentes = await _context.Colaborador
            .Where(c => ids.Contains(c.ColaboradorId))
            .Select(c => c.ColaboradorId)
            .ToListAsync();

        var idsFaltantes = ids.Except(colaboradoresExistentes).ToList();
        if (idsFaltantes.Any())
            throw new KeyNotFoundException(
                $"No existen los colaboradores con id: {string.Join(", ", idsFaltantes)}."
            );

        if (!string.IsNullOrEmpty(vacante.Estado) &&
            !vacante.Estado.Equals("Abierta", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "No se pueden crear postulaciones para una vacante que no está abierta.");
        }

        var rankingActual = await _matchingService.GetRankedCandidatesAsync(dto.VacanteId);

        var existentes = await _repository.GetByVacanteIdAsync(dto.VacanteId);
        var idsYaPostulados = existentes.Select(p => p.ColaboradorId).ToHashSet();

        var nuevasPostulaciones = new List<Postulacion>();

        foreach (var colabId in ids)
        {
            if (idsYaPostulados.Contains(colabId))
                continue;

            var datosCandidato = rankingActual.FirstOrDefault(r => r.ColaboradorId == colabId);
            double scoreReal = datosCandidato?.PorcentajeMatch ?? 0;

            nuevasPostulaciones.Add(new Postulacion
            {
                VacanteId = dto.VacanteId,
                ColaboradorId = colabId,
                Estado = "En Revision",
                FechaPostulacion = DateTime.Now,
                MatchScore = (decimal?)scoreReal
            });
        }

        if (nuevasPostulaciones.Any())
        {
            foreach (var p in nuevasPostulaciones)
                await _repository.AddAsync(p);

            await _repository.SaveChangesAsync();
        }

        return await GetPorVacanteAsync(dto.VacanteId);
    }

    public async Task<PostulacionDto> CambiarEstadoAsync(int postulacionId, CambiarEstadoDto dto)
    {
        using var tx = await _context.Database.BeginTransactionAsync();

        try
        {
            var postulacion = await _repository.GetByIdAsync(postulacionId);
            if (postulacion == null)
                throw new KeyNotFoundException("Postulación no encontrada");

            // 1) Cambiar estado
            postulacion.Estado = dto.NuevoEstado;
            if (!string.IsNullOrEmpty(dto.Comentarios))
                postulacion.Comentarios = dto.Comentarios;

            await _repository.UpdateAsync(postulacion);

            // 2) Si se selecciona candidato => lógica full negocio
            if (dto.NuevoEstado.Equals("Seleccionado", StringComparison.OrdinalIgnoreCase))
            {
                var vacante = await _context.Vacante
                    .FirstOrDefaultAsync(v => v.VacanteId == postulacion.VacanteId);

                if (vacante == null)
                    throw new KeyNotFoundException("Vacante no encontrada");

                if (!vacante.Estado.Equals("Abierta", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("La vacante no está abierta.");

                var colaborador = await _context.Colaborador
                    .FirstOrDefaultAsync(c => c.ColaboradorId == postulacion.ColaboradorId);

                if (colaborador == null)
                    throw new KeyNotFoundException("Colaborador no encontrado");

                // ✅ A) Cerrar vacante
                vacante.Estado = "Cerrada";
                vacante.Descripcion = (vacante.Descripcion ?? "") +
                                      $"\n[Sistema]: Cerrada por contratación de candidato ID {colaborador.ColaboradorId}.";

                // ✅ B) Marcar colaborador como NO disponible para movilidad
                colaborador.DisponibleMovilidad = false;

                // ✅ C) Actualizar Area/Departamento si la vacante lo define
                if (vacante.AreaId.HasValue)
                    colaborador.AreaId = vacante.AreaId.Value;

                if (vacante.DepartamentoId.HasValue)
                    colaborador.DepartamentoId = vacante.DepartamentoId.Value;

                // ✅ D) Opcional: activo true (si quieres forzarlo)
                if (colaborador.Activo == null)
                    colaborador.Activo = true;

                _context.Vacante.Update(vacante);
                _context.Colaborador.Update(colaborador);

                // ✅ E) (Opcional fuerte y recomendado)
                // Rechazar automáticamente a otros candidatos en esa vacante
                var otras = await _repository.GetAllByVacanteIdAsync(vacante.VacanteId);

                foreach (var p in otras)
                {
                    if (p.PostulacionId == postulacion.PostulacionId)
                        continue;

                    if (p.Estado != null &&
                        p.Estado.Equals("Seleccionado", StringComparison.OrdinalIgnoreCase))
                        continue;

                    p.Estado = "Rechazado";
                    p.Comentarios = "Rechazado automáticamente: vacante cubierta.";
                    _context.Postulacion.Update(p);
                }
            }

            // 3) Guardar todo
            await _repository.SaveChangesAsync();
            await tx.CommitAsync();

            return MapToDto(postulacion);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }


    public async Task<PostulacionDto> RechazarAsync(int postulacionId, string motivo)
    {
        return await CambiarEstadoAsync(postulacionId, new CambiarEstadoDto
        {
            NuevoEstado = "Rechazado",
            Comentarios = motivo
        });
    }

    public async Task<IEnumerable<PostulacionDto>> GetPorVacanteAsync(int vacanteId)
    {
        var lista = await _repository.GetByVacanteIdAsync(vacanteId);
        return lista.Select(MapToDto);
    }

    public async Task<IEnumerable<PostulacionDto>> GetPorEstadoAsync(string estado)
    {
        var lista = await _repository.GetByEstadoAsync(estado);
        return lista.Select(MapToDto);
    }

    private static PostulacionDto MapToDto(Postulacion p)
    {
        return new PostulacionDto
        {
            PostulacionId = p.PostulacionId,
            VacanteId = p.VacanteId,
            VacanteTitulo = p.Vacante?.Titulo ?? "N/A",
            ColaboradorId = p.ColaboradorId,
            NombreColaborador = p.Colaborador != null
                ? $"{p.Colaborador.Nombres} {p.Colaborador.Apellidos}"
                : "N/A",
            Estado = p.Estado ?? "Desconocido",
            MatchScore = p.MatchScore,
            FechaPostulacion = p.FechaPostulacion ?? DateTime.MinValue,
            Comentarios = p.Comentarios
        };
    }
}
