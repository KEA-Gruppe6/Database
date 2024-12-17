using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class AirportController : ControllerBase
{
    private readonly IAirportService _airportService;

    public AirportController(IAirportService airportService)
    {
        _airportService = airportService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Airport?>> GetAirport(int id)
    {
        var airport = await _airportService.GetAirportByIdAsync(id);
        if (airport == null)
        {
            return NotFound($"Airport with ID {id} not found.");
        }

        return Ok(airport);
    }

    [HttpPost]
    public async Task<ActionResult<Airport>> CreateAirport([FromBody] AirportRequestDTO airportDTO)
    {
        Airport airport = new Airport
        {
            AirportId = 0,
            AirportName = airportDTO.AirportName,
            AirportCity = airportDTO.AirportCity,
            Municipality = airportDTO.Municipality,
            AirportAbbreviation = airportDTO.AirportAbbreviation
        };

        var createdAirport = await _airportService.CreateAirportAsync(airport);
        return CreatedAtAction(nameof(GetAirport), new { id = createdAirport.AirportId }, createdAirport);
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<Airport>> UpdateAirport(long id, [FromBody] AirportRequestDTO updatedAirportDTO)
    {
        Airport updatedAirport = new Airport
        {
            AirportId = id,
            AirportName = updatedAirportDTO.AirportName,
            AirportCity = updatedAirportDTO.AirportCity,
            Municipality = updatedAirportDTO.Municipality,
            AirportAbbreviation = updatedAirportDTO.AirportAbbreviation
        };

        try
        {
            var result = await _airportService.UpdateAirportAsync(updatedAirport);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Airport>> DeleteAirport(int id)
    {
        try
        {
            var result = await _airportService.DeleteAirportAsync(id);
            return Ok(result);
        }
        catch (DbUpdateException e)
        {
            var errorMessage = e.InnerException?.Message ?? e.Message;

            if (errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"table ""[^""]+\.(?<table>[^""]+)"", column '(?<column>[^']+)'");
                var match = regex.Match(errorMessage);

                if (match.Success)
                {

                    var table = match.Groups["table"].Value;
                    var column = match.Groups["column"].Value;
                    return BadRequest($"Failed to delete. Object linked to column '{column}' in table '{table}'.");
                }
            }

            return BadRequest(errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}