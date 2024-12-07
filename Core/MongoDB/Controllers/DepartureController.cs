using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;


[ApiController]
[Route("MongoDB/[controller]")]
public class DepartureController : ControllerBase
{
    private readonly DepartureService _departureService;

    public DepartureController(DepartureService departureService)
    {
        _departureService = departureService;
    }

    [HttpGet("{id}", Name = "GetDeparture")]
    public async Task<IActionResult> GetDeparture(string id)
    {
        var departure = await _departureService.GetDepartureByIdAsync(id);

        if (departure == null)
        {
            return NotFound(new { Message = $"MongoDBDeparture with ID {id} not found." });
        }

        return Ok(departure);
    }

    [HttpPost(Name = "CreateDeparture")]
    public async Task<IActionResult> CreateDeparture([FromBody] MongoDBDepartureDTO departureDTO)
    {
        if (departureDTO == null)
        {
            return BadRequest(new { Message = "Invalid departure data." });
        }
        
        var departure = new MongoDBDeparture
        {
            DepartureTime = departureDTO.DepartureTime,
            ArrivalTime = departureDTO.ArrivalTime,
            DepartureAirportId = departureDTO.DepartureAirportId,
            ArrivalAirportId = departureDTO.ArrivalAirportId,
            TicketIds = departureDTO.TicketIds
        };

        await _departureService.CreateDepartureAsync(departure);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBDeparture created successfully.", departure = departure });
    }

    [HttpPatch("{id}", Name = "UpdateDeparture")]
    public async Task<IActionResult> UpdateDeparture(string id, [FromBody] MongoDBDepartureDTO departureDTO)
    {
        if (departureDTO == null)
        {
            return BadRequest(new { Message = "Invalid departure data." });
        }
        
        var existingDeparture = await _departureService.GetDepartureByIdAsync(id);

        if (existingDeparture == null)
        {
            return NotFound(new { Message = $"MongoDBDeparture with ID {id} not found." });
        }
        
        var departure = new MongoDBDeparture
        {
            DepartureTime = departureDTO.DepartureTime,
            ArrivalTime = departureDTO.ArrivalTime,
            DepartureAirportId = departureDTO.DepartureAirportId,
            ArrivalAirportId = departureDTO.ArrivalAirportId,
            TicketIds = departureDTO.TicketIds
        };

        await _departureService.UpdateDepartureAsync(id, departure);
        return Ok(new { Message = "MongoDBDeparture updated successfully.", departure = departure });
    }

    [HttpDelete("{id}", Name = "DeleteDeparture")]
    public async Task<IActionResult> DeleteDeparture(string id)
    {
        var existingDeparture = await _departureService.GetDepartureByIdAsync(id);

        if (existingDeparture == null)
        {
            return NotFound(new { Message = $"MongoDBDeparture with ID {id} not found." });
        }

        await _departureService.DeleteDepartureAsync(id);
        return Ok(new { Message = "MongoDBDeparture deleted successfully." });
    }
}