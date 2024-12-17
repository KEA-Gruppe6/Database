using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4j.Controllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket([FromBody] Ticket ticket)
        {
            try
            {
                var createdTicket = await _ticketService.CreateTicketAsync(ticket);
                return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.TicketId }, createdTicket);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsByOrderId(long orderId)
        {
            var tickets = await _ticketService.GetTicketsByOrderIdAsync(orderId);
            return Ok(tickets);
        }

        [HttpDelete("{ticketId}")]
        public async Task<IActionResult> DeleteTicket(long ticketId)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(ticketId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(long id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }
    }
}
