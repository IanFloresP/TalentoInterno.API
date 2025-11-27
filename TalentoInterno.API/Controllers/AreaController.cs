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
    public class AreaController : Controller
    {
        private readonly TalentoInternooContext _context;
        public AreaController(TalentoInternooContext context)
        {
            _context = context;
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

        [HttpPost("areas")]
        public async Task<IActionResult> CreateArea([FromBody] AreaDto areaDto)
        {
            var nombre = areaDto.Nombre?.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Nombre is required.");

            var exists = await _context.Area.AnyAsync(a => a.Nombre == nombre);
            if (exists) return Conflict("An area with that Nombre already exists.");

            var area = new CORE.Core.Entities.Area
            {
                Nombre = nombre,
                DepartamentoId = areaDto.DepartamentoId
            };

            _context.Area.Add(area);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // handle rare race condition: unique constraint triggered despite pre-check
                return Conflict("An area with that Nombre already exists.");
            }

            areaDto.AreaId = area.AreaId;
            return CreatedAtAction(nameof(GetAreas), new { id = areaDto.AreaId }, areaDto);
        }

        [HttpPut("areas/{id}")] // HU-12
        public async Task<IActionResult> UpdateArea(int id, [FromBody] AreaDto areaDto)
        {
            var area = await _context.Area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            area.Nombre = areaDto.Nombre;
            area.DepartamentoId = areaDto.DepartamentoId;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("areas/{id}")] // HU-12
        public async Task<IActionResult> DeleteArea(int id)
        {
            var area = await _context.Area.FindAsync(id);
            if (area == null)
            {
                return NotFound();
            }
            _context.Area.Remove(area);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
