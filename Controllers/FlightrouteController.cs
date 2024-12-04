using Database_project.Controllers.RequestDTOs;
using Database_project.Core.DTOs;
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
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}