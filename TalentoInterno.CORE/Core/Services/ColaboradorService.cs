using Microsoft.EntityFrameworkCore;
using System.Linq;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.CORE.Core.Services;

public class ColaboradorService : IColaboradorService
{
    private readonly IColaboradorRepository _repository;
    private readonly TalentoInternooContext _context;

    public ColaboradorService(IColaboradorRepository repository, TalentoInternooContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<ColaboradorDTO>> GetAllColaboradoresAsync()
    {
        var colaboradores = await _repository.GetAllAsync();
        return colaboradores.Select(c => new ColaboradorDTO
        {
            ColaboradorId = c.ColaboradorId,
            DNI = c.DNI ?? "",
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
            DNI = colaborador.DNI ?? "",
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

    public async Task<Colaborador> CreateColaboradorAsync(ColaboradorCreateDTO dto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Crear el Colaborador (Sin contraseña)
            var colaborador = new Colaborador
            {
                DNI = dto.DNI,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                RolId = dto.RolId,
                AreaId = dto.AreaId,
                DepartamentoId = dto.DepartamentoId, // Assuming int? is handled
                DisponibleMovilidad = dto.DisponibleMovilidad,
                Activo = dto.Activo,
                FechaAlta = DateOnly.FromDateTime(DateTime.Now)
            };

            await _repository.AddAsync(colaborador);

            // 2. Crear el Usuario (Con la contraseña hasheada)
            var usuario = new Usuario
            {
                Email = dto.Email,
                // Usamos BCrypt para hashear la contraseña que viene en el CreateDTO
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = dto.RolId,
                ColaboradorId = colaborador.ColaboradorId,
                Activo = true,
                FechaCreacion = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return colaborador;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // ... (Update and Delete remain the same)


    public async Task UpdateColaboradorAsync(ColaboradorDTO dto)
    {
        // 1. Buscar al Colaborador existente
        var colaborador = await _repository.GetByIdAsync(dto.ColaboradorId);
        if (colaborador == null) throw new KeyNotFoundException("Colaborador no encontrado.");

        // --- ACTUALIZACIÓN DE DATOS DE COLABORADOR ---
        colaborador.DNI = dto.DNI;
        colaborador.Nombres = dto.Nombres;
        colaborador.Apellidos = dto.Apellidos;
        colaborador.Email = dto.Email;
        colaborador.RolId = dto.RolId;
        colaborador.AreaId = dto.AreaId;
        colaborador.DepartamentoId = dto.DepartamentoId;
        colaborador.DisponibleMovilidad = dto.DisponibleMovilidad;
        colaborador.Activo = dto.Activo;

        // ¡IMPORTANTE! NO actualizamos FechaAlta. Se queda la que tenía.

        // 2. Buscar el Usuario asociado (para actualizar credenciales)
        // Usamos el _context directamente porque es una operación cruzada
        var usuario = await _context.Usuario
            .FirstOrDefaultAsync(u => u.ColaboradorId == colaborador.ColaboradorId);

        if (usuario != null)
        {
            // Sincronizar Email (si cambiaron el email del colaborador, cambiamos el del login)
            usuario.Email = dto.Email;

            // Sincronizar Rol (si el DTO trae rol, actualizamos el acceso)
            usuario.RolId = dto.RolId;

            // Sincronizar Estado
            usuario.Activo = dto.Activo;

            // --- ACTUALIZACIÓN DE CONTRASEÑA (Lógica Segura) ---
            // Solo si el campo 'Contraseña' del DTO NO está vacío, la actualizamos.
            // Si viene null o "", asumimos que no quieren cambiarla.
            if (!string.IsNullOrEmpty(dto.Contraseña))
            {
                usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contraseña);
            }
        }

        // 3. Guardar todo (Colaborador y Usuario)
        // Al usar _repository.UpdateAsync, EF detecta cambios en 'colaborador'.
        // Como 'usuario' también está traqueado por el contexto, se guardará al hacer SaveChanges.
        await _repository.UpdateAsync(colaborador);
        // Nota: Tu repositorio ya debería hacer SaveChangesAsync(), si no, añádelo aquí.
    }

    public async Task DeleteColaboradorAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}