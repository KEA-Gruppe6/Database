using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class AirlineController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirlineController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }


    [HttpGet("{id:long}", Name = "Airline")]
    public async Task<IActionResult> GetAirline(long id)
    {
        var airline = await _airlineService.GetAirlineByIdAsync(id);
        if (airline == null)
        {
            return NotFound($"Airline with ID {id} not found.");
        }

        return Ok(airline);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAirline([FromBody] Airline airline)
    {
        var createdAirline = await _airlineService.CreateAirlineAsync(airline);
        return CreatedAtAction(nameof(GetAirline), new { id = createdAirline.AirlineId }, createdAirline);
    }


    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateAirline(long id, [FromBody] Airline updatedAirline)
    {
        if (id != updatedAirline.AirlineId)
        {
            return BadRequest("Airline ID mismatch.");
        }

        var result = await _airlineService.UpdateAirlineAsync(updatedAirline);
        if (!result)
        {
            return NotFound($"Airline with ID {id} not found.");
        }

        return Ok(updatedAirline);
    }


    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAirline(long id)
    {
        var result = await _airlineService.DeleteAirlineAsync(id);
        if (!result)
        {
            return NotFound($"Airline with ID {id} not found.");
        }

        return NoContent();
    }
}