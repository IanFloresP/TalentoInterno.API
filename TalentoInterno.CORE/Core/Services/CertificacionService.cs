using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class CertificacionService : ICertificacionService
{
    private readonly ICertificacionRepository _repository;

    public CertificacionService(ICertificacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CertificacionDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(c => new CertificacionDto
        {
            CertificacionId = c.CertificacionId,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion
        });
    }

    public async Task<CertificacionDto?> GetByIdAsync(int id)
    {
        var c = await _repository.GetByIdAsync(id);
        if (c == null) return null;

        return new CertificacionDto
        {
            CertificacionId = c.CertificacionId,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion
        };
    }

    public async Task<CertificacionDto> CreateAsync(CertificacionCreateDto dto)
    {
        var entidad = new Certificacion
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        await _repository.AddAsync(entidad);
        await _repository.SaveChangesAsync();

        return new CertificacionDto
        {
            CertificacionId = entidad.CertificacionId,
            Nombre = entidad.Nombre,
            Descripcion = entidad.Descripcion
        };
    }

    public async Task UpdateAsync(int id, CertificacionUpdateDto dto)
    {
        var entidad = await _repository.GetByIdAsync(id);
        if (entidad == null) throw new KeyNotFoundException("Certificación no encontrada");

        entidad.Nombre = dto.Nombre;
        entidad.Descripcion = dto.Descripcion;

        await _repository.UpdateAsync(entidad);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}