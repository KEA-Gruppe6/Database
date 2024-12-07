using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;


[ApiController]
[Route("MongoDB/[controller]")]
public class PlaneController : ControllerBase
{
    private readonly PlaneService _planeService;

    public PlaneController(PlaneService planeService)
    {
        _planeService = planeService;
    }

    [HttpGet("{id}", Name = "GetPlane")]
    public async Task<IActionResult> GetPlane(string id)
    {
        var plane = await _planeService.GetPlaneByIdAsync(id);

        if (plane == null)
        {
            return NotFound(new { Message = $"MongoDBPlane with ID {id} not found." });
        }

        return Ok(plane);
    }

    [HttpPost(Name = "CreatePlane")]
    public async Task<IActionResult> CreatePlane([FromBody] MongoDBPlaneDTO planeDTO)
    {
        if (planeDTO == null)
        {
            return BadRequest(new { Message = "Invalid plane data." });
        }
        
        var plane = new MongoDBPlane
        {
            PlaneDisplayName = planeDTO.PlaneDisplayName,
            AirlineId = planeDTO.AirlineId,
            Departures = planeDTO.Departures
        };

        await _planeService.CreatePlaneAsync(plane);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBPlane created successfully.", plane = plane });
    }

    [HttpPatch("{id}", Name = "UpdatePlane")]
    public async Task<IActionResult> UpdatePlane(string id, [FromBody] MongoDBPlaneDTO planeDTO)
    {
        if (planeDTO == null)
        {
            return BadRequest(new { Message = "Invalid plane data." });
        }
        
        var existingPlane = await _planeService.GetPlaneByIdAsync(id);

        if (existingPlane == null)
        {
            return NotFound(new { Message = $"MongoDBPlane with ID {id} not found." });
        }
        
        var plane = new MongoDBPlane
        {
            PlaneDisplayName = planeDTO.PlaneDisplayName,
            AirlineId = planeDTO.AirlineId,
            Departures = planeDTO.Departures
        };

        await _planeService.UpdatePlaneAsync(id, plane);
        return Ok(new { Message = "MongoDBPlane updated successfully.", plane = plane });
    }

    [HttpDelete("{id}", Name = "DeletePlane")]
    public async Task<IActionResult> DeletePlane(string id)
    {
        var existingPlane = await _planeService.GetPlaneByIdAsync(id);

        if (existingPlane == null)
        {
            return NotFound(new { Message = $"MongoDBPlane with ID {id} not found." });
        }

        await _planeService.DeletePlaneAsync(id);
        return Ok(new { Message = "MongoDBPlane deleted successfully." });
    }
}