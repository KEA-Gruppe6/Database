using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class PlaneController : ControllerBase
{
    private IPlaneService _planeService;

    public PlaneController(IPlaneService planeService)
    {
        _planeService = planeService;
    }

    [HttpGet()]
    public async Task<ActionResult<List<PlaneDTO_Airline>>> GetPlanes()
    {
        var planes = await _planeService.GetPlanesAsync();

        return Ok(planes);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PlaneDTO_Airline?>> GetPlane(long id)
    {
        var plane = await _planeService.GetPlaneByIdAsync(id);
        if (plane == null)
        {
            return NotFound($"Plane with ID {id} not found.");
        }

        return Ok(plane);
    }

    [HttpPost]
    public async Task<ActionResult<PlaneDTO_Airline>> CreatePlane([FromBody] PlaneRequestDTO planeDTO)
    {
        Plane plane = new Plane
        {
            PlaneId = 0,
            PlaneDisplayName = planeDTO.PlaneDisplayName,
            AirlineId = planeDTO.AirlineId,
        };

        try
        {
            var createdPlane = await _planeService.CreatePlaneAsync(plane);
            return CreatedAtAction(nameof(GetPlane), new { id = createdPlane.PlaneId }, createdPlane);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<PlaneDTO_Airline>> UpdatePlane(long id, [FromBody] PlaneRequestDTO updatedPlaneDTO)
    {
        Plane updatedPlane = new Plane
        {
            PlaneId = id,
            PlaneDisplayName = updatedPlaneDTO.PlaneDisplayName,
            AirlineId = updatedPlaneDTO.AirlineId,
        };

        try
        {
            var result = await _planeService.UpdatePlaneAsync(updatedPlane);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Plane>> DeletePlane(long id)
    {
        try
        {
            var result = await _planeService.DeletePlaneAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}