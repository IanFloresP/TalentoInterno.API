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
    public class UrgenciaController : Controller
    {
        private readonly TalentoInternooContext _context;
        public UrgenciaController(TalentoInternooContext context)
        {
            _context = context;
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
        [HttpPost("urgencias")]
        public async Task<IActionResult> CreateUrgencia([FromBody] UrgenciaDto urgenciaDto)
        {
            var urgencia = new CORE.Core.Entities.Urgencia
            {
                Nombre = urgenciaDto.Nombre
            };
            _context.Urgencia.Add(urgencia);
            await _context.SaveChangesAsync();
            urgenciaDto.UrgenciaId = urgencia.UrgenciaId;
            return CreatedAtAction(nameof(GetUrgencias), new { id = urgenciaDto.UrgenciaId }, urgenciaDto);
        }
        [HttpPut("urgencias/{id}")]
        public async Task<IActionResult> UpdateUrgencia(int id, [FromBody] UrgenciaDto urgenciaDto)
        {
            var urgencia = await _context.Urgencia.FindAsync(id);
            if (urgencia == null)
            {
                return NotFound();
            }
            urgencia.Nombre = urgenciaDto.Nombre;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("urgencias/{id}")]
        public async Task<IActionResult> DeleteUrgencia(int id)
        {
            var urgencia = await _context.Urgencia.FindAsync(id);
            if (urgencia == null)
            {
                return NotFound();
            }
            _context.Urgencia.Remove(urgencia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
