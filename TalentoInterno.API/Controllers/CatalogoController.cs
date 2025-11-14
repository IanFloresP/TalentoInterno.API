using Microsoft.AspNetCore.Mvc;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs; // ¡Importar DTOs!
using System.Linq; // ¡Importar LINQ para .Select()!

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/catalogos")]
public class CatalogoController : ControllerBase
{
    private readonly TalentoInternooContext _context;

    public CatalogoController(TalentoInternooContext context)
    {
        _context = context;
    }

    [HttpGet("proyectos")]
    public async Task<IActionResult> GetProyectos()
    {
        var data = await _context.Proyecto
            .Select(p => new ProyectoDto
            {
                ProyectoId = p.ProyectoId,
                Nombre = p.Nombre,
                CuentaId = p.CuentaId
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("cuentas")]
    public async Task<IActionResult> GetCuentas()
    {
        var data = await _context.Cuenta
            .Select(c => new CuentaDto
            {
                CuentaId = c.CuentaId,
                Nombre = c.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("perfiles")]
    public async Task<IActionResult> GetPerfiles()
    {
        var data = await _context.Perfil
            .Select(p => new PerfilDto
            {
                PerfilId = p.PerfilId,
                Nombre = p.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("urgencias")]
    public async Task<IActionResult> GetUrgencias()
    {
        var data = await _context.Urgencia
            .Select(u => new UrgenciaDto
            {
                UrgenciaId = u.UrgenciaId,
                Nombre = u.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("niveles")]
    public async Task<IActionResult> GetNiveles()
    {
        var data = await _context.NivelDominio
            .Select(n => new NivelDominioDto
            {
                NivelId = n.NivelId,
                Nombre = n.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("departamentos")]
    public async Task<IActionResult> GetDepartamentos()
    {
        var data = await _context.Departamento
            .Select(d => new DepartamentoDto
            {
                DepartamentoId = d.DepartamentoId,
                Nombre = d.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }

    [HttpGet("tiposkill")]
    public async Task<IActionResult> GetTiposSkill()
    {
        var data = await _context.TipoSkill
            .Select(t => new TipoSkillDto
            {
                TipoSkillId = t.TipoSkillId,
                Nombre = t.Nombre
            })
            .ToListAsync();
        return Ok(data);
    }
    [HttpGet("areas")] // HU-11, HU-12
    public async Task<IActionResult> GetAreas()
    {
        var data = await _context.Area
            .Select(a => new AreaDto
            {
                AreaId = a.AreaId,
                Nombre = a.Nombre,
                DepartamentoId = a.DepartamentoId
            })
            .ToListAsync();
        return Ok(data);
    }
}