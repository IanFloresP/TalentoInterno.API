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
    public class PerfilController : Controller
    {
        private readonly TalentoInternooContext _context;
        public PerfilController(TalentoInternooContext context)
        {
            _context = context;
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
        [HttpPost("perfiles")]
        public async Task<IActionResult> CreatePerfil([FromBody] PerfilDto perfilDto)
        {
            var perfil = new CORE.Core.Entities.Perfil
            {
                Nombre = perfilDto.Nombre
            };
            _context.Perfil.Add(perfil);
            await _context.SaveChangesAsync();
            perfilDto.PerfilId = perfil.PerfilId;
            return CreatedAtAction(nameof(GetPerfiles), new { id = perfilDto.PerfilId }, perfilDto);
        }

        [HttpPut("perfiles/{id}")]
        public async Task<IActionResult> UpdatePerfil(int id, [FromBody] PerfilDto perfilDto)
        {
            var perfil = await _context.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            perfil.Nombre = perfilDto.Nombre;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("perfiles/{id}")]
        public async Task<IActionResult> DeletePerfil(int id)
        {
            var perfil = await _context.Perfil.FindAsync(id);
            if (perfil == null)
            {
                return NotFound();
            }
            _context.Perfil.Remove(perfil);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
