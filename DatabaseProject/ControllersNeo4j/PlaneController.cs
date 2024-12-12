using Database_project.Core.Neo4j;
using Database_project.Core.Neo4j.Entities;
using Database_project.Core.Neo4j.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace Database_project.Controllers.Neo4j;

[ApiController]
[Route("api/neo4j/[controller]")]
public class PlaneController : ControllerBase
{
    private IPlaneService _planeService;

    public PlaneController(IPlaneService planeService)
    {
        _planeService = planeService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var planes = await _planeService.GetPlanesAsync();

        return Ok(planes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var plane = await _planeService.GetPlaneByIdAsync(id);

        return Ok(plane);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Plane plane)
    {
        var createdPlane = await _planeService.CreatePlaneAsync(plane);

        return Ok(createdPlane);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(long id, [FromBody] Plane plane)
    {
        var updatedPlane = await _planeService.UpdatePlaneAsync(id, plane);

        return Ok(updatedPlane);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var deletedPlane = await _planeService.DeletePlaneAsync(id);

        return Ok(deletedPlane);
    }
}
