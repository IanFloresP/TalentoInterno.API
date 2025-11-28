using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class PostulacionService : IPostulacionService
{
    private readonly IPostulacionRepository _repository;

    public PostulacionService(IPostulacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<PostulacionDto> CrearAsync(CrearPostulacionDto dto)
    {
        // 1. Validar si ya existe postulación (Opcional pero recomendado)
        var existentes = await _repository.GetByColaboradorIdAsync(dto.ColaboradorId);
        if (existentes.Any(p => p.VacanteId == dto.VacanteId))
        {
            throw new InvalidOperationException("El colaborador ya está postulado a esta vacante.");
        }

        var postulacion = new Postulacion
        {
            VacanteId = dto.VacanteId,
            ColaboradorId = dto.ColaboradorId,
            MatchScore = dto.MatchScore,
            Estado = "En Revision",
            FechaPostulacion = DateTime.Now
        };

        await _repository.AddAsync(postulacion);
        await _repository.SaveChangesAsync();

        // Retornamos el DTO recargando la entidad para traer nombres
        var guardado = await _repository.GetByIdAsync(postulacion.PostulacionId);
        return MapToDto(guardado!);
    }

    public async Task<PostulacionDto> CambiarEstadoAsync(int id, string nuevoEstado, string? comentarios)
    {
        var postulacion = await _repository.GetByIdAsync(id);
        if (postulacion == null) throw new KeyNotFoundException("Postulación no encontrada");

        postulacion.Estado = nuevoEstado;
        if (!string.IsNullOrEmpty(comentarios))
        {
            postulacion.Comentarios = comentarios;
        }

        await _repository.UpdateAsync(postulacion);
        await _repository.SaveChangesAsync();

        return MapToDto(postulacion);
    }

    public async Task<IEnumerable<PostulacionDto>> GetPorVacanteAsync(int vacanteId)
    {
        var postulaciones = await _repository.GetByVacanteIdAsync(vacanteId);
        return postulaciones.Select(MapToDto);
    }

    // Helper privado para mapear
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