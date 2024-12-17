using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class PlaneController : ControllerBase
    {
        private readonly PlaneService _planeService;
        public PlaneController(PlaneService planeService)
        {
            _planeService = planeService;
        }

        // GET: api/<PlaneController>
        [HttpGet("{flightrouteId}")]
        public async Task<ActionResult<Plane>> GetByFlightrouteId(long flightrouteId)
        {
            var planes = await _planeService.GetPlaneByFlightrouteIdAsync(flightrouteId);
            return Ok(planes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlane(long id)
        {
            await _planeService.DeletePlaneAsync(id);
            return NoContent();
        }
    }
}
