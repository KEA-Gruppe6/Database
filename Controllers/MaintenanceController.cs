using Database_project.Controllers.RequestDTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpGet("{id:long}", Name = "Maintenance")]
    public async Task<IActionResult> GetMaintenance(long id)
    {
        try
        {
            var maintenance = await _maintenanceService.GetMaintenanceByIdAsync(id);
            return Ok(maintenance);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaintenance([FromBody] MaintenanceRequestDTO maintenanceDTO)
    {
        Maintenance maintenance = new Maintenance
        {
            MaintenanceId = 0,
            AirportId = maintenanceDTO.AirportId,
            StartDate = maintenanceDTO.StartDate,
            EndDate = maintenanceDTO.EndDate,
            PlaneId = maintenanceDTO.PlaneId
        };
        try
        {
            var createdMaintenance = await _maintenanceService.CreateMaintenanceAsync(maintenance);
            return CreatedAtAction(nameof(GetMaintenance), new { id = createdMaintenance.MaintenanceId }, createdMaintenance);
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateMaintenance([FromBody] MaintenanceRequestDTO updatedMaintenanceDTO)
    {
        Maintenance updatedMaintenance = new Maintenance
        {
            MaintenanceId = updatedMaintenanceDTO.MaintenanceId,
            AirportId = updatedMaintenanceDTO.AirportId,
            StartDate = updatedMaintenanceDTO.StartDate,
            EndDate = updatedMaintenanceDTO.EndDate,
            PlaneId = updatedMaintenanceDTO.PlaneId
        };

        try
        {
            var result = await _maintenanceService.UpdateMaintenanceAsync(updatedMaintenance);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMaintenance(int id)
    {
        var result = await _maintenanceService.DeleteMaintenanceAsync(id);
        if (!result)
        {
            return NotFound($"Maintenance with ID {id} not found.");
        }

        return NoContent();
    }
}