using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class FlightrouteController : ControllerBase
{
    private readonly IFlightrouteService _flightrouteService;

    public FlightrouteController(IFlightrouteService flightrouteService)
    {
        _flightrouteService = flightrouteService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<FlightrouteDTO?>> GetFlightroute(long id)
    {
        var flightroute = await _flightrouteService.GetFlightrouteByIdAsync(id);
        if (flightroute == null)
        {
            return NotFound($"Flightroute with ID {id} not found.");
        }

        return Ok(flightroute);
    }

    [HttpPost]
    public async Task<ActionResult<FlightrouteDTO>> CreateFlightroute([FromBody] FlightrouteRequestDTO flightrouteDTO)
    {
        Flightroute flightroute = new Flightroute
        {
            FlightrouteId = 0,
            DepartureAirportId = flightrouteDTO.DepartureAirportId,
            ArrivalAirportId = flightrouteDTO.ArrivalAirportId,
            ArrivalTime = flightrouteDTO.ArrivalTime,
            DepartureTime = flightrouteDTO.DepartureTime,
            PlaneId = flightrouteDTO.PlaneId
        };

        try
        {
            var createdFlightroute = await _flightrouteService.CreateFlightrouteAsync(flightroute);
            return CreatedAtAction(nameof(GetFlightroute), new { id = createdFlightroute.FlightrouteId }, createdFlightroute);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<FlightrouteDTO>> UpdateFlightroute(long id, [FromBody] FlightrouteRequestDTO updatedFlightrouteDTO)
    {
        Flightroute updatedFlightroute = new Flightroute
        {
            FlightrouteId = id,
            DepartureAirportId = updatedFlightrouteDTO.DepartureAirportId,
            ArrivalAirportId = updatedFlightrouteDTO.ArrivalAirportId,
            ArrivalTime = updatedFlightrouteDTO.ArrivalTime,
            DepartureTime = updatedFlightrouteDTO.DepartureTime,
            PlaneId = updatedFlightrouteDTO.PlaneId
        };

        try
        {
            var result = await _flightrouteService.UpdateFlightrouteAsync(updatedFlightroute);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Flightroute>> DeleteFlightroute(long id)
    {
        try
        {
            var result = await _flightrouteService.DeleteFlightrouteAsync(id);
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