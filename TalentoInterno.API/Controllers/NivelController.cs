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
    public class NivelController : Controller
    {
        private readonly TalentoInternooContext _context;
        public NivelController(TalentoInternooContext context)
        {
            _context = context;
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
        [HttpPost("niveles")]
        public async Task<IActionResult> CreateNivel([FromBody] NivelDominioDto nivelDto)
        {
            var nivel = new CORE.Core.Entities.NivelDominio
            {
                Nombre = nivelDto.Nombre
            };
            _context.NivelDominio.Add(nivel);
            await _context.SaveChangesAsync();
            nivelDto.NivelId = nivel.NivelId;
            return CreatedAtAction(nameof(GetNiveles), new { id = nivelDto.NivelId }, nivelDto);
        }
        [HttpPut("niveles/{id}")]
        public async Task<IActionResult> UpdateNivel(int id, [FromBody] NivelDominioDto nivelDto)
        {
            var nivel = await _context.NivelDominio.FindAsync(id);
            if (nivel == null)
            {
                return NotFound();
            }
            nivel.Nombre = nivelDto.Nombre;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("niveles/{id}")]
        public async Task<IActionResult> DeleteNivel(int id)
        {
            var nivel = await _context.NivelDominio.FindAsync(id);
            if (nivel == null)
            {
                return NotFound();
            }
            _context.NivelDominio.Remove(nivel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
