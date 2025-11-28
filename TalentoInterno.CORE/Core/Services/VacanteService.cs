using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentoInterno.CORE.Core.Services;

public class VacanteService : IVacanteService
{
    private readonly IVacanteRepository _vacanteRepo;
    private readonly TalentoInternooContext _context;

    public VacanteService(IVacanteRepository vacanteRepo, TalentoInternooContext context)
    {
        _vacanteRepo = vacanteRepo;
        _context = context;
    }

    public async Task<VacanteGetDTO?> GetVacanteByIdAsync(int id)
    {
        var vacante = await _context.Vacante
            .Include(v => v.Perfil)
            .Include(v => v.Cuenta)
            .Include(v => v.Proyecto)
            .Include(v => v.Urgencia)
            .Where(v => v.VacanteId == id)
            .Select(v => new VacanteGetDTO
            {
                VacanteId = v.VacanteId,
                Titulo = v.Titulo,
                Estado = v.Estado,
                Descripcion = v.Descripcion,

                // --- CORRECCIÓN 1 (CS0266) ---
                FechaInicio = v.FechaInicio.GetValueOrDefault(),
                PerfilNombre = v.Perfil == null ? null:  v.Perfil.Nombre,
                CuentaNombre = v.Cuenta == null ? null : v.Cuenta.Nombre,
                ProyectoNombre = v.Proyecto == null ? null : v.Proyecto.Nombre,
                UrgenciaNombre = v.Urgencia == null ? null : v.Urgencia.Nombre
            })
            .FirstOrDefaultAsync();

        return vacante;
    }

    public async Task<IEnumerable<VacanteListDTO>> GetAllVacantesAsync(int? perfilId = null, int? areaId = null, int? departamentoId = null)
    {
        // 1. Preparar la consulta con Includes
        var query = _context.Vacante
            .Include(v => v.Perfil)
            .Include(v => v.Urgencia)
            .Include(v => v.Area)         // Asegúrate de que Vacante tenga estas relaciones
            .Include(v => v.Departamento) // Si no, haz Scaffold de nuevo
            .AsQueryable();

        // 2. Aplicar Filtros Dinámicos
        if (perfilId.HasValue)
            query = query.Where(v => v.PerfilId == perfilId.Value);

        if (areaId.HasValue)
            query = query.Where(v => v.AreaId == areaId.Value);

        // --- NUEVO FILTRO POR DEPARTAMENTO ---
        if (departamentoId.HasValue)
            query = query.Where(v => v.DepartamentoId == departamentoId.Value);
        // -------------------------------------

        // 3. Proyectar al DTO
        return await query.Select(v => new VacanteListDTO
        {
            VacanteId = v.VacanteId,
            Titulo = v.Titulo,
            Estado = v.Estado,
            FechaInicio = v.FechaInicio.GetValueOrDefault(),

            PerfilNombre = v.Perfil == null ? null : v.Perfil.Nombre,
            UrgenciaNombre = v.Urgencia == null ? null : v.Urgencia.Nombre,

            // Mapear nombres de Área y Depto
            AreaNombre = v.Area == null ? null : v.Area.Nombre,
            DepartamentoNombre = v.Departamento == null ? null : v.Departamento.Nombre
        }).ToListAsync();
    }

    public async Task<Vacante> CreateVacanteAsync(VacanteCreateDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var vacante = new Vacante
            {
                Titulo = dto.Titulo,
                PerfilId = dto.PerfilId,

                // --- NUEVAS ASIGNACIONES ---
                AreaId = dto.AreaId,
                DepartamentoId = dto.DepartamentoId,
                // ---------------------------

                CuentaId = dto.CuentaId,
                ProyectoId = dto.ProyectoId,
                FechaInicio = dto.FechaInicio,
                UrgenciaId = (byte)dto.UrgenciaId,
                Estado = dto.Estado,
                Descripcion = dto.Descripcion
            };

            await _vacanteRepo.AddAsync(vacante);

            if (dto.Skills != null && dto.Skills.Any())
            {
                foreach (var skillDto in dto.Skills)
                {
                    var req = new VacanteSkillReq
                    {
                        VacanteId = vacante.VacanteId,
                        SkillId = skillDto.SkillId,

                        // --- CORRECCIÓN 5 (CS0266) ---
                        // Asumimos que NivelDeseado en el DTO es 'int' o 'byte?'
                        // y que en la entidad es 'byte'
                        NivelDeseado = (byte)skillDto.NivelDeseado,

                        Peso = skillDto.Peso,
                        Critico = skillDto.Critico
                    };
                    await _context.VacanteSkillReq.AddAsync(req);
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return vacante;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateVacanteAsync(int id, VacanteUpdateDTO dto)
    {
        var vacante = await _vacanteRepo.GetByIdAsync(id);
        if (vacante == null) throw new KeyNotFoundException("Vacante no encontrada");

        // Mapear DTO a entidad
        vacante.Titulo = dto.Titulo;
        vacante.PerfilId = dto.PerfilId;

        // --- NUEVAS ASIGNACIONES ---
        vacante.AreaId = dto.AreaId;
        vacante.DepartamentoId = dto.DepartamentoId;
        // ---------------------------

        vacante.CuentaId = dto.CuentaId;
        vacante.ProyectoId = dto.ProyectoId;
        vacante.FechaInicio = dto.FechaInicio;
        vacante.UrgenciaId = (byte)dto.UrgenciaId;
        vacante.Estado = dto.Estado;
        vacante.Descripcion = dto.Descripcion;

        await _vacanteRepo.UpdateAsync(vacante);
        await _vacanteRepo.SaveChangesAsync();
    }

    public async Task DeleteVacanteAsync(int id)
    {
        await _vacanteRepo.DeleteAsync(id);
    }
    public async Task CerrarVacanteAsync(int vacanteId)
    {
        var vacante = await _vacanteRepo.GetByIdAsync(vacanteId);
        if (vacante == null) throw new KeyNotFoundException("Vacante no encontrada");
        vacante.Estado = "Cerrada"; // O el estado que corresponda
        await _vacanteRepo.UpdateAsync(vacante);
    }
}