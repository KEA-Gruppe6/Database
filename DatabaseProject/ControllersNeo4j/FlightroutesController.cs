using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.ControllersNeo4j
{
    [ApiController]
    [Route("api/neo4j/[controller]")]
    public class FlightrouteController : ControllerBase
    {
        private readonly FlightrouteService _flightrouteService;

        public FlightrouteController(FlightrouteService flightrouteService)
        {
            _flightrouteService = flightrouteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlightroute([FromBody] Flightroute flightroute)
        {
            if (flightroute == null)
            {
                return BadRequest("Flightroute is null.");
            }

            var createdFlightroute = await _flightrouteService.CreateFlightrouteAsync(flightroute);
            return CreatedAtAction(nameof(GetFlightrouteById), new { id = createdFlightroute.FlightrouteId }, createdFlightroute);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlightrouteById(long id)
        {
            var flightroute = await _flightrouteService.GetFlightrouteByFlightrouteIdAsync(id);
            if (flightroute == null)
            {
                return NotFound();
            }

            return Ok(flightroute);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFlightroute(long id)
        {
            await _flightrouteService.DeleteFlightrouteAsync(id);
            return NoContent();
        }
    }
}
