using Microsoft.EntityFrameworkCore;
using System.Linq;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Services;

public class AuditoriaService : IAuditoriaService
{
    private readonly IAuditoriaRepository _repository;
    private readonly TalentoInternooContext _context;

    public AuditoriaService(IAuditoriaRepository repository, TalentoInternooContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task RegistrarAccionAsync(AuditoriaCreateDto dto)
    {
        var auditoria = new Auditoria
        {
            UsuarioId = dto.UsuarioId,
            Accion = dto.Accion,
            Entidad = dto.Entidad,
            EntidadId = dto.EntidadId,
            Detalle = dto.Detalle,
            Exitoso = dto.Exitoso,
            Fecha = DateTime.Now
        };

        await _repository.AddAsync(auditoria);
        await _repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditoriaGetDto>> ObtenerHistorialAsync(int? usuarioId, DateTime? desde, DateTime? hasta, string? accion)
    {
        var historial = await _repository.GetFiltradoAsync(usuarioId, desde, hasta, accion);

        return historial.Select(a => new AuditoriaGetDto
        {
            AuditoriaId = a.AuditoriaId,
            UsuarioEmail = a.Usuario?.Email ?? "Sistema/Anónimo",
            Accion = a.Accion,
            Entidad = a.Entidad,
            EntidadId = a.EntidadId,
            Detalle = a.Detalle,
            Fecha = a.Fecha ?? DateTime.MinValue,
            Exitoso = a.Exitoso
        });
    }
    public async Task<IEnumerable<AuditoriaResumenDto>> ObtenerResumenAsync()
    {
        // Agrupamos por UsuarioId para contar cuántas acciones hizo cada uno
        var resumen = await _context.Auditoria
            .Include(a => a.Usuario)
            .GroupBy(a => a.UsuarioId)
            .Select(g => new AuditoriaResumenDto
            {
                Usuario = g.First().Usuario != null ? g.First().Usuario.Email : "Sistema/Anónimo",
                CantidadAcciones = g.Count(),
                UltimaAccion = g.OrderByDescending(a => a.Fecha).First().Accion,
                FechaUltimaAccion = g.OrderByDescending(a => a.Fecha).First().Fecha ?? DateTime.MinValue

            })
            .OrderByDescending(r => r.CantidadAcciones)
            .ToListAsync();

        return resumen;
    }
}