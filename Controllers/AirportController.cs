using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportController : ControllerBase
{
    private readonly IAirportService _airportService;

    public AirportController(IAirportService airportService)
    {
        _airportService = airportService;
    }

    [HttpGet("{id:long}", Name = "Airport")]
    public async Task<IActionResult> GetAirport(int id)
    {
        var airport = await _airportService.GetAirportByIdAsync(id);
        if (airport == null)
        {
            return NotFound($"Airport with ID {id} not found.");
        }

        return Ok(airport);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAirport([FromBody] Airport airport)
    {
        var createdAirport = await _airportService.CreateAirportAsync(airport);
        return CreatedAtAction(nameof(GetAirport), new { id = createdAirport.AirportId }, createdAirport);
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateAirport(long id, [FromBody] Airport updatedAirport)
    {

        var result = await _airportService.UpdateAirportAsync(id, updatedAirport);
        if (!result)
        {
            return NotFound($"Airport with ID {id} not found.");
        }

        updatedAirport.AirportId = id;
        return Ok(updatedAirport);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAirport(int id)
    {
        var result = await _airportService.DeleteAirportAsync(id);
        if (!result)
        {
            return NotFound($"Airport with ID {id} not found.");
        }

        return NoContent();
    }
}