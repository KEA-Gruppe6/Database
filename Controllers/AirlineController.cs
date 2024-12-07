using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Database_project.Controllers.RequestDTOs;
using Database_project.Core.DTOs;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")] //TODO: Change all api routes to /api/mssql/...
public class AirlineController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirlineController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }


    [HttpGet("{id:long}")]
    public async Task<ActionResult<AirlineDTO?>> GetAirline(long id)
    {
        var airline = await _airlineService.GetAirlineByIdAsync(id);
        if (airline == null)
        {
            return NotFound($"Airline with ID {id} not found.");
        }

        return Ok(airline);
    }


    [HttpPost()]
    public async Task<ActionResult<AirlineDTO>> CreateAirline([FromBody] AirlineRequestDTO airlineDTO)
    {
        Airline airline = new Airline
        {
            AirlineId = 0,
            AirlineName = airlineDTO.AirlineName,
            Planes = null
        };

        var createdAirline = await _airlineService.CreateAirlineAsync(airline);
        return CreatedAtAction(nameof(GetAirline), new { id = createdAirline.AirlineId }, createdAirline);
    }


    [HttpPatch("{id:long}")]
    public async Task<ActionResult<AirlineDTO>> UpdateAirline(long id, [FromBody] AirlineRequestDTO updatedAirlineDTO)
    {
        Airline updatedAirline = new Airline
        {
            AirlineId = id,
            AirlineName = updatedAirlineDTO.AirlineName,
        };

        try
        {
            var result = await _airlineService.UpdateAirlineAsync(updatedAirline);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Airline>> DeleteAirline(long id)
    {
        try
        {
            var result = await _airlineService.DeleteAirlineAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}