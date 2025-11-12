using Microsoft.AspNetCore.Mvc;

namespace TalentoInterno.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        // Implement logic for HU-14
        return Ok();
    }

    [HttpPost]
    public IActionResult Create()
    {
        // Implement logic for HU-14
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id)
    {
        // Implement logic for HU-14
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        // Implement logic for HU-14
        return Ok();
    }
}