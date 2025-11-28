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
    private readonly IMatchingService _matchingService; // <--- ¡La clave profesional!
    private readonly IVacanteRepository _vacanteRepo; // Para cerrar la vacante
    private readonly TalentoInternooContext _context; // Para transacciones

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
        // 1. Validar que la vacante exista y esté abierta
        var vacante = await _vacanteRepo.GetByIdAsync(dto.VacanteId);
        if (vacante == null) throw new Exception("Vacante no encontrada.");
        if (vacante.Estado != "Abierta") throw new Exception("No se puede postular a una vacante cerrada.");

        // 2. Obtener el Ranking Actualizado (La fuente de la verdad)
        var rankingActual = await _matchingService.GetRankedCandidatesAsync(dto.VacanteId);

        // 3. Obtener postulaciones existentes para no duplicar
        var existentes = await _repository.GetByVacanteIdAsync(dto.VacanteId);
        var idsYaPostulados = existentes.Select(p => p.ColaboradorId).ToHashSet();

        var nuevasPostulaciones = new List<Postulacion>();

        foreach (var colabId in dto.ColaboradorIds)
        {
            if (idsYaPostulados.Contains(colabId)) continue; // Saltar si ya existe

            // Buscar el score real en el ranking
            var datosCandidato = rankingActual.FirstOrDefault(r => r.ColaboradorId == colabId);
            double scoreReal = datosCandidato?.PorcentajeMatch ?? 0;

            var postulacion = new Postulacion
            {
                VacanteId = dto.VacanteId,
                ColaboradorId = colabId,
                Estado = "En Revisión", // Estado inicial
                FechaPostulacion = DateTime.Now,
                MatchScore = (decimal?)scoreReal // Guardamos el score histórico
            };

            nuevasPostulaciones.Add(postulacion);
        }

        if (nuevasPostulaciones.Any())
        {
            // Guardamos todo en lote
            foreach (var p in nuevasPostulaciones) await _repository.AddAsync(p);
            await _repository.SaveChangesAsync();
        }

        return await GetPorVacanteAsync(dto.VacanteId);
    }

    public async Task<PostulacionDto> CambiarEstadoAsync(int postulacionId, CambiarEstadoDto dto)
    {
        var postulacion = await _repository.GetByIdAsync(postulacionId);
        if (postulacion == null) throw new KeyNotFoundException("Postulación no encontrada");

        // Actualizar estado
        postulacion.Estado = dto.NuevoEstado;
        if (!string.IsNullOrEmpty(dto.Comentarios)) postulacion.Comentarios = dto.Comentarios;

        await _repository.UpdateAsync(postulacion);

        // --- LÓGICA DE NEGOCIO: CIERRE DE VACANTE ---
        if (dto.NuevoEstado == "Seleccionado")
        {
            var vacante = await _vacanteRepo.GetByIdAsync(postulacion.VacanteId);
            if (vacante != null && vacante.Estado == "Abierta")
            {
                vacante.Estado = "Cerrada"; // Cerramos la vacante automáticamente
                vacante.Descripcion += $"\n[Sistema]: Cerrada por contratación de candidato ID {postulacion.ColaboradorId}.";
                await _vacanteRepo.UpdateAsync(vacante);
            }

            // Opcional: Aquí podrías rechazar automáticamente a los demás candidatos de esta vacante
        }
        // ---------------------------------------------

        await _repository.SaveChangesAsync();
        return MapToDto(postulacion);
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

    private static PostulacionDto MapToDto(Postulacion p)
    {
        return new PostulacionDto
        {
            PostulacionId = p.PostulacionId,
            VacanteId = p.VacanteId,
            VacanteTitulo = p.Vacante?.Titulo ?? "N/A",
            ColaboradorId = p.ColaboradorId,
            NombreColaborador = p.Colaborador != null ? $"{p.Colaborador.Nombres} {p.Colaborador.Apellidos}" : "N/A",
            Estado = p.Estado ?? "Desconocido",
            MatchScore = p.MatchScore,
            FechaPostulacion = p.FechaPostulacion ?? DateTime.MinValue,
            Comentarios = p.Comentarios
        };
    }
}