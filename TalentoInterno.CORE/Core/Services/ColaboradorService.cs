using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.DTOs;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _repository;

    public ColaboradorService(IColaboradorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ColaboradorDTO>> GetAllColaboradoresAsync()
    {
        var colaboradores = await _repository.GetAllAsync();
        return colaboradores.Select(c => new ColaboradorDTO
        {
            ColaboradorId = c.ColaboradorId,
            Nombres = c.Nombres,
            Apellidos = c.Apellidos,
            Email = c.Email,
            RolId = c.RolId,
            AreaId = c.AreaId,
            DepartamentoId = c.DepartamentoId, // Conversión segura de int? a int
            DisponibleMovilidad = c.DisponibleMovilidad,
            Activo = c.Activo,
            FechaAlta = c.FechaAlta,
            AreaNombre = c.Area?.Nombre,
            DepartamentoNombre = c.Departamento?.Nombre,
            RolNombre = c.Rol?.Nombre,
            Certificaciones = c.ColaboradorCertificacion?.Select(cc => cc.Certificacion.Nombre),
            Skills = c.ColaboradorSkill?.Select(cs => cs.Skill.Nombre)
        });
    }

    public async Task<ColaboradorDTO?> GetColaboradorByIdAsync(int id)
    {
        var colaborador = await _repository.GetByIdAsync(id);
        if (colaborador == null) return null;

        return new ColaboradorDTO
        {
            ColaboradorId = colaborador.ColaboradorId,
            Nombres = colaborador.Nombres,
            Apellidos = colaborador.Apellidos,
            Email = colaborador.Email,
            RolId = colaborador.RolId,
            AreaId = colaborador.AreaId,
            DepartamentoId = colaborador.DepartamentoId, // Conversión segura de int? a int
            DisponibleMovilidad = colaborador.DisponibleMovilidad,
            Activo = colaborador.Activo,
            FechaAlta = colaborador.FechaAlta,
            AreaNombre = colaborador.Area?.Nombre,
            DepartamentoNombre = colaborador.Departamento?.Nombre,
            RolNombre = colaborador.Rol?.Nombre,
            Certificaciones = colaborador.ColaboradorCertificacion?.Select(cc => cc.Certificacion.Nombre),
            Skills = colaborador.ColaboradorSkill?.Select(cs => cs.Skill.Nombre)
        };
    }

    public async Task<Colaborador> CreateColaboradorAsync(ColaboradorDTO colaboradorDTO)
    {
        // 1. Crea una nueva entidad Colaborador
        var colaborador = new Colaborador
        {
            // 2. Mapea los datos desde el DTO
            Nombres = colaboradorDTO.Nombres,
            Apellidos = colaboradorDTO.Apellidos,
            Email = colaboradorDTO.Email,
            RolId = colaboradorDTO.RolId,
            AreaId = colaboradorDTO.AreaId,
            DepartamentoId = colaboradorDTO.DepartamentoId,
            DisponibleMovilidad = colaboradorDTO.DisponibleMovilidad,
            Activo = colaboradorDTO.Activo,

            // 3. ¡TU REQUISITO! Establece la fecha de alta como la fecha actual
            FechaAlta = DateOnly.FromDateTime(DateTime.Now)
        };

        // 4. Llama al repositorio para guardar
        await _repository.AddAsync(colaborador);

        // 5. Devuelve la entidad completa (con el nuevo ColaboradorId)
        return colaborador;
    }

    public async Task UpdateColaboradorAsync(ColaboradorDTO colaboradorDTO)
    {
        // Opcional: Primero busca el colaborador existente
        var colaborador = await _repository.GetByIdAsync(colaboradorDTO.ColaboradorId);
        if (colaborador == null)
        {
            throw new Exception("Colaborador no encontrado"); // O maneja el error
        }

        // Mapea los valores del DTO a la entidad
        colaborador.Nombres = colaboradorDTO.Nombres;
        colaborador.Apellidos = colaboradorDTO.Apellidos;
        colaborador.Email = colaboradorDTO.Email;
        colaborador.RolId = colaboradorDTO.RolId;
        colaborador.AreaId = colaboradorDTO.AreaId;
        colaborador.DepartamentoId = colaboradorDTO.DepartamentoId;
        colaborador.DisponibleMovilidad = colaboradorDTO.DisponibleMovilidad;
        colaborador.Activo = colaboradorDTO.Activo;
        colaborador.FechaAlta = colaboradorDTO.FechaAlta;

        // Llama al repositorio con la entidad actualizada
        await _repository.UpdateAsync(colaborador);
    }

    public async Task DeleteColaboradorAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}