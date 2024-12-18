using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        private readonly AirlineService _airlineService;
        public AirlinesController(AirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        // GET: api/<AirlinesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Airline>>> Get()
        {
            var airlines = await _airlineService.GetAllAirlinesAsync();
            return Ok(airlines);
        }

        // GET api/<AirlinesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<Airline>> Get(long id)
        {
            var airline = await _airlineService.GetAirlineByIdAsync(id);
            return Ok(airline);
        }

        // POST api/<AirlinesController>
        [HttpPost]
        public async Task Post([FromBody] Airline airline)
        {
            await _airlineService.AddAirlineAsync(airline);
        }

        // PUT api/<AirlinesController>/5
        [HttpPut("{id}")]
        public async void Put(long id, [FromBody] Airline value)
        {
            await _airlineService.UpdateAirlineAsync(id, value);
        }

        // DELETE api/<AirlinesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(long id)
        {
            await _airlineService.DeleteAirlineAsync(id);
        }
    }
}
