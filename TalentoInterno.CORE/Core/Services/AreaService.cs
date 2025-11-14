using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _repository;

    public AreaService(IAreaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AreaDto>> GetAllAreasAsync()
    {
        var areas = await _repository.GetAllAsync();
        return areas.Select(a => new AreaDto
        {
            AreaId = a.AreaId,
            Nombre = a.Nombre,
            DepartamentoId = a.DepartamentoId
        });
    }

    public async Task<AreaDto?> GetAreaByIdAsync(int id)
    {
        var area = await _repository.GetByIdAsync(id);
        if (area == null) return null;

        return new AreaDto
        {
            AreaId = area.AreaId,
            Nombre = area.Nombre,
            DepartamentoId = area.DepartamentoId
        };
    }

    public async Task<AreaDto> CreateAreaAsync(AreaDto areaDto)
    {
        var area = new Area
        {
            Nombre = areaDto.Nombre,
            DepartamentoId = areaDto.DepartamentoId
        };

        await _repository.AddAsync(area);
        await _repository.SaveChangesAsync();

        areaDto.AreaId = area.AreaId; // Devolvemos el nuevo ID
        return areaDto;
    }

    public async Task UpdateAreaAsync(int id, AreaDto areaDto)
    {
        var existingArea = await _repository.GetByIdAsync(id);
        if (existingArea == null)
        {
            throw new KeyNotFoundException("Área no encontrada.");
        }

        existingArea.Nombre = areaDto.Nombre;
        existingArea.DepartamentoId = areaDto.DepartamentoId;

        await _repository.UpdateAsync(existingArea);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAreaAsync(int id)
    {
        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }
}