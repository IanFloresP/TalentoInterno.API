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
    public class CuentaController : ControllerBase

    {
        private readonly TalentoInternooContext _context;
        public CuentaController(TalentoInternooContext context)
        {
            _context = context;
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
        [HttpPost("cuentas")]
        public async Task<IActionResult> CreateCuenta([FromBody] CuentaDto cuentaDto)
        {
            var cuenta = new CORE.Core.Entities.Cuenta
            {
                Nombre = cuentaDto.Nombre
            };
            _context.Cuenta.Add(cuenta);
            await _context.SaveChangesAsync();
            cuentaDto.CuentaId = cuenta.CuentaId;
            return CreatedAtAction(nameof(GetCuentas), new { id = cuentaDto.CuentaId }, cuentaDto);
        }
        [HttpPut("cuentas/{id}")]
        public async Task<IActionResult> UpdateCuenta(int id, [FromBody] CuentaDto cuentaDto)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            cuenta.Nombre = cuentaDto.Nombre;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("cuentas/{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            _context.Cuenta.Remove(cuenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
