using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Infrastructure.Data;

namespace TalentoInterno.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin, RRHH, Business Manager")]
    public class ProyectoController : Controller
    {

        private readonly TalentoInternooContext _context;

        public ProyectoController(TalentoInternooContext context)
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

        [HttpPost("proyectos")]
        public async Task<IActionResult> CreateProyecto([FromBody] ProyectoDto proyectoDto)
        {
            var proyecto = new CORE.Core.Entities.Proyecto
            {
                Nombre = proyectoDto.Nombre,
                CuentaId = proyectoDto.CuentaId
            };
            _context.Proyecto.Add(proyecto);
            await _context.SaveChangesAsync();
            proyectoDto.ProyectoId = proyecto.ProyectoId;
            return CreatedAtAction(nameof(GetProyectos), new { id = proyectoDto.ProyectoId }, proyectoDto);
        }
        [HttpPut("proyectos/{id}")]
        public async Task<IActionResult> UpdateProyecto(int id, [FromBody] ProyectoDto proyectoDto)
        {
            var proyecto = await _context.Proyecto.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }
            proyecto.Nombre = proyectoDto.Nombre;
            proyecto.CuentaId = proyectoDto.CuentaId;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("proyectos/{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await _context.Proyecto.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }
            _context.Proyecto.Remove(proyecto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
