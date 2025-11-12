using Microsoft.AspNetCore.Mvc;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColaboradorController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        // Implement logic for HU-09, HU-11, HU-12
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        // Implement logic for HU-09, HU-11
        return Ok();
    }

    [HttpGet("{id}/rol")]
    public IActionResult GetRol(int id)
    {
        // Implement logic for HU-14
        return Ok();
    }
}