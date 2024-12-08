using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("api/MongoDB/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceService _maintenanceService;

    public MaintenanceController(MaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpGet("{id}", Name = "GetMaintenance")]
    public async Task<IActionResult> GetMaintenance(string id)
    {
        var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

        if (maintenance == null)
        {
            return NotFound(new { Message = $"MongoDBMaintenance with ID {id} not found." });
        }

        return Ok(maintenance);
    }

    [HttpPost(Name = "CreateMaintenance")]
    public async Task<IActionResult> CreateMaintenance([FromBody] MongoDBMaintenanceDTO maintenanceDTO)
    {
        if (maintenanceDTO == null)
        {
            return BadRequest(new { Message = "Invalid maintenance data." });
        }
        
        var maintenance = new MongoDBMaintenance
        {
            AirportId = maintenanceDTO.AirportId,
            PlaneId = maintenanceDTO.PlaneId
        };

        await _maintenanceService.CreateMaintenanceAsync(maintenance);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBMaintenance created successfully.", maintenance = maintenance });
    }

    [HttpPatch("{id}", Name = "UpdateMaintenance")]
    public async Task<IActionResult> UpdateMaintenance(string id, [FromBody] MongoDBMaintenanceDTO maintenanceDTO)
    {
        if (maintenanceDTO == null)
        {
            return BadRequest(new { Message = "Invalid maintenance data." });
        }
        
        var existingMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

        if (existingMaintenance == null)
        {
            return NotFound(new { Message = $"MongoDBMaintenance with ID {id} not found." });
        }
        
        var maintenance = new MongoDBMaintenance
        {
            AirportId = maintenanceDTO.AirportId,
            PlaneId = maintenanceDTO.PlaneId
        };

        await _maintenanceService.UpdateMaintenanceAsync(id, maintenance);
        return Ok(new { Message = "MongoDBMaintenance updated successfully.", maintenance = maintenance });
    }

    [HttpDelete("{id}", Name = "DeleteMaintenance")]
    public async Task<IActionResult> DeleteMaintenance(string id)
    {
        var existingMaintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);

        if (existingMaintenance == null)
        {
            return NotFound(new { Message = $"MongoDBMaintenance with ID {id} not found." });
        }

        await _maintenanceService.DeleteMaintenanceAsync(id);
        return Ok(new { Message = "MongoDBMaintenance deleted successfully." });
    }
}