using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;


[ApiController]
[Route("api/MongoDB/[controller]")]
public class FlightrouteController : ControllerBase
{
    private readonly FlightrouteService _flightrouteService;

    public FlightrouteController(FlightrouteService flightrouteService)
    {
        _flightrouteService = flightrouteService;
    }

    [HttpGet("{id}", Name = "GetFlightroute")]
    public async Task<IActionResult> GetFlightroute(string id)
    {
        var flightroute = await _flightrouteService.GetFlightrouteByIdAsync(id);

        if (flightroute == null)
        {
            return NotFound(new { Message = $"MongoDBFlightroute with ID {id} not found." });
        }

        return Ok(flightroute);
    }

    [HttpPost(Name = "CreateFlightroute")]
    public async Task<IActionResult> CreateFlightroute([FromBody] MongoDBFlightrouteDTO flightrouteDTO)
    {
        if (flightrouteDTO == null)
        {
            return BadRequest(new { Message = "Invalid flightroute data." });
        }
        
        var flightroute = new MongoDBFlightroute
        {
            DepartureTime = flightrouteDTO.DepartureTime,
            ArrivalTime = flightrouteDTO.ArrivalTime,
            DepartureAirportId = flightrouteDTO.DepartureAirportId,
            ArrivalAirportId = flightrouteDTO.ArrivalAirportId,
            TicketIds = flightrouteDTO.TicketIds
        };

        await _flightrouteService.CreateFlightrouteAsync(flightroute);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBFlightroute created successfully.", flightroute = flightroute });
    }

    [HttpPatch("{id}", Name = "UpdateFlightroute")]
    public async Task<IActionResult> UpdateFlightroute(string id, [FromBody] MongoDBFlightrouteDTO flightrouteDTO)
    {
        if (flightrouteDTO == null)
        {
            return BadRequest(new { Message = "Invalid flightroute data." });
        }
        
        var existingFlightroute = await _flightrouteService.GetFlightrouteByIdAsync(id);

        if (existingFlightroute == null)
        {
            return NotFound(new { Message = $"MongoDBFlightroute with ID {id} not found." });
        }
        
        var flightroute = new MongoDBFlightroute
        {
            DepartureTime = flightrouteDTO.DepartureTime,
            ArrivalTime = flightrouteDTO.ArrivalTime,
            DepartureAirportId = flightrouteDTO.DepartureAirportId,
            ArrivalAirportId = flightrouteDTO.ArrivalAirportId,
            TicketIds = flightrouteDTO.TicketIds
        };

        await _flightrouteService.UpdateFlightrouteAsync(id, flightroute);
        return Ok(new { Message = "MongoDBFlightroute updated successfully.", flightroute = flightroute });
    }

    [HttpDelete("{id}", Name = "DeleteFlightroute")]
    public async Task<IActionResult> DeleteFlightroute(string id)
    {
        var existingFlightroute = await _flightrouteService.GetFlightrouteByIdAsync(id);

        if (existingFlightroute == null)
        {
            return NotFound(new { Message = $"MongoDBFlightroute with ID {id} not found." });
        }

        await _flightrouteService.DeleteFlightrouteAsync(id);
        return Ok(new { Message = "MongoDBFlightroute deleted successfully." });
    }
}