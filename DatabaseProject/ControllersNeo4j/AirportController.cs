using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportService _airportService;
        public AirportController(AirportService airportService)
        {
            _airportService = airportService;
        }

        // GET: api/<AirportController>
        [HttpGet("{flightrouteId}")]
        public async Task<ActionResult<IEnumerable<Airport>>> GetAirportsByFlightrouteId(long flightrouteId)
        {
            var airports = await _airportService.GetAirportsForFlightrouteAsync(flightrouteId);
            return Ok(airports);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(long id)
        {
            await _airportService.DeleteAirportAsync(id);
            return NoContent();
        }
    }
}
