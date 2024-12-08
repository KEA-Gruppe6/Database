using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("api/MongoDB/[controller]")]
public class AirlineController : ControllerBase
{
    private readonly AirlineService _airlineService;

    public AirlineController(AirlineService airlineService)
    {
        _airlineService = airlineService;
    }

    [HttpGet("{id}", Name = "GetAirline")]
    public async Task<IActionResult> GetAirline(string id)
    {
        var airline = await _airlineService.GetAirlineByIdAsync(id);

        if (airline == null)
        {
            return NotFound(new { Message = $"MongoDBAirline with ID {id} not found." });
        }

        return Ok(airline);
    }

    [HttpPost(Name = "CreateAirline")]
    public async Task<IActionResult> CreateAirline([FromBody] MongoDBAirlineDTO airlineDTO)
    {
        if (airlineDTO == null)
        {
            return BadRequest(new { Message = "Invalid airline data." });
        }
        
        var airline = new MongoDBAirline
        {
            AirlineName = airlineDTO.AirlineName,
            Planes = airlineDTO.Planes,
        };

        await _airlineService.CreateAirlineAsync(airline);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBAirline created successfully.", airline = airline });
    }

    [HttpPatch("{id}", Name = "UpdateAirline")]
    public async Task<IActionResult> UpdateAirline(string id, [FromBody] MongoDBAirlineDTO airlineDTO)
    {
        if (airlineDTO == null)
        {
            return BadRequest(new { Message = "Invalid airline data." });
        }
        
        var existingAirline = await _airlineService.GetAirlineByIdAsync(id);

        if (existingAirline == null)
        {
            return NotFound(new { Message = $"MongoDBAirline with ID {id} not found." });
        }
        
        var airline = new MongoDBAirline
        {
            AirlineName = airlineDTO.AirlineName,
            Planes = airlineDTO.Planes,
        };

        await _airlineService.UpdateAirlineAsync(id, airline);
        return Ok(new { Message = "MongoDBAirline updated successfully.", airline = airline });
    }

    [HttpDelete("{id}", Name = "DeleteAirline")]
    public async Task<IActionResult> DeleteAirline(string id)
    {
        var existingAirline = await _airlineService.GetAirlineByIdAsync(id);

        if (existingAirline == null)
        {
            return NotFound(new { Message = $"MongoDBAirline with ID {id} not found." });
        }

        await _airlineService.DeleteAirlineAsync(id);
        return Ok(new { Message = "MongoDBAirline deleted successfully." });
    }
}