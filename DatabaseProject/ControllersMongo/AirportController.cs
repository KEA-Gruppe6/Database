using Database_project.Core.MongoDB.RequestDTOs;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("api/MongoDB/[controller]")]
public class AirportController : ControllerBase
{
    private readonly AirportService _airportService;

    public AirportController(AirportService airportService)
    {
        _airportService = airportService;
    }

    [HttpGet("{id}", Name = "GetAirportById")]
    public async Task<IActionResult> GetAirport(string id)
    {
        var airport = await _airportService.GetAirportByIdAsync(id);
        if (airport == null)
        {
            return NotFound(new { message = $"Airport with ID {id} not found." });
        }

        return Ok(airport);
    }

    [HttpGet(Name = "GetAllAirports")]
    public async Task<IActionResult> GetAllAirports()
    {
        var airports = await _airportService.GetAirportAsync();
        return Ok(airports);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAirport([FromBody] MongoDBAirportDTO airportDTO)
    {

        var airport = new MongoDBAirport()
        {
            AirportName = airportDTO.AirportName,
            AirportCity = airportDTO.AirportCity,
            Municipality = airportDTO.Municipality,
            AirportAbbreviation = airportDTO.AirportAbbreviation
        };


        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _airportService.CreateAirportAsync(airport);
        return CreatedAtAction(nameof(GetAirport), new { id = airport.AirportId }, airport);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAirport(string id, [FromBody] MongoDBAirport updatedAirport)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _airportService.UpdateAirportAsync(id, updatedAirport);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAirport(string id)
    {
        var airport = await _airportService.GetAirportByIdAsync(id);
        if (airport == null)
        {
            return NotFound(new { message = $"Airport with ID {id} not found." });
        }

        await _airportService.DeleteAirportAsync(id);
        return NoContent();
    }
}
