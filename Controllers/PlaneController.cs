using Database_project.Core.Entities;
using Database_project.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaneController : ControllerBase
{
    private PlaneService _planeService;

    public PlaneController(PlaneService planeService)
    {
        _planeService = planeService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Plane?>> GetPlane(long id)
    {
        try
        {
            var plane = await _planeService.GetPlaneByIdAsync(id);  
            return Ok(plane);
        }
        catch (Exception ex)
        {
            return NotFound($"Plane with ID {id} not found.");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Plane>> CreatePlane([FromBody] Plane plane)
    {
        try
        {
            var createdPlane = await _planeService.CreatePlaneAsync(plane);

            return Created("api/Plane/" + createdPlane.PlaneId, createdPlane);

        }
        catch (Exception)
        {
            return BadRequest("Plane data is invalid.");
        }
    }

    [HttpPatch]
    public async Task<ActionResult<Plane?>> UpdatePlane([FromBody] Plane plane)
    {
        try
        {
            var updatedPlane = await _planeService.UpdatePlaneAsync(plane);
            return Ok(updatedPlane);
        }
        catch (Exception ex)
        {
            return BadRequest("Plane data is invalid.");
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<bool>> DeletePlane(long id)
    {
        try
        {
            var isDeleted = await _planeService.DeletePlaneByIdAsync(id);

            return Ok(isDeleted);
        }
        catch (Exception)
        {
            return BadRequest("Plane data is invalid.");
        }
    }
}