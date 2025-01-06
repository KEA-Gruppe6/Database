using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class LuggageController : ControllerBase
    {
        private readonly LuggageService _luggageService;
        public LuggageController(LuggageService luggageService)
        {
            _luggageService = luggageService;
        }

        // GET: api/<LuggageController>
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<IEnumerable<Luggage>>> GetByTicket(long ticketId)
        {
            var luggage = await _luggageService.GetLuggageByTicketIdAsync(ticketId);
            return Ok(luggage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLuggage(long id)
        {
            await _luggageService.DeleteLuggageAsync(id);
            return NoContent();
        }
    }
}
