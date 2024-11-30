using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightrouteController : ControllerBase
{
    private readonly IFlightrouteService _flightrouteService;

    public FlightrouteController(IFlightrouteService flightrouteService)
    {
        _flightrouteService = flightrouteService;
    }

    [HttpGet("{id:long}", Name = "Flightroute")]
    public async Task<IActionResult> GetFlightroute(long id)
    {
        var flightroute = await _flightrouteService.GetFlightrouteByIdAsync(id);
        if (flightroute == null)
        {
            return NotFound($"Flightroute with ID {id} not found.");
        }

        return Ok(flightroute);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFlightroute([FromBody] Flightroute flightroute)
    {
        Flightroute createdFlightroute;
        try
        {
            createdFlightroute = await _flightrouteService.CreateFlightrouteAsync(flightroute);
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(nameof(GetFlightroute), new { id = createdFlightroute.FlightrouteId }, createdFlightroute);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateFlightroute(long id, [FromBody] Flightroute updatedFlightroute)
    {
        if (id != updatedFlightroute.FlightrouteId)
        {
            return BadRequest("Flightroute ID mismatch.");
        }

        var result = await _flightrouteService.UpdateFlightrouteAsync(updatedFlightroute);
        if (!result)
        {
            return NotFound($"Flightroute with ID {id} not found.");
        }

        return Ok(updatedFlightroute);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFlightroute(int id)
    {
        var result = await _flightrouteService.DeleteFlightrouteAsync(id);
        if (!result)
        {
            return NotFound($"Flightroute with ID {id} not found.");
        }

        return NoContent();
    }
}