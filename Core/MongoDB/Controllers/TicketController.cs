using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;


[ApiController]
[Route("api/MongoDB/[controller]")]
public class TicketController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id}", Name = "GetTicket")]
    public async Task<IActionResult> GetTicket(string id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);

        if (ticket == null)
        {
            return NotFound(new { Message = $"MongoDBTicket with ID {id} not found." });
        }

        return Ok(ticket);
    }

    [HttpPost(Name = "CreateTicket")]
    public async Task<IActionResult> CreateTicket([FromBody] MongoDBTicketDTO ticketDTO)
    {
        if (ticketDTO == null)
        {
            return BadRequest(new { Message = "Invalid ticket data." });
        }
        
        var ticket = new MongoDBTicket
        {
            Price = ticketDTO.Price,
            TicketType = ticketDTO.TicketType,
            CustomerId = ticketDTO.CustomerId,
            FlightrouteId = ticketDTO.FlightrouteId,
            OrderId = ticketDTO.OrderId,
            LuggageIds = ticketDTO.LuggageIds
        };

        await _ticketService.CreateTicketAsync(ticket);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBTicket created successfully.", ticket = ticket });
    }

    [HttpPatch("{id}", Name = "UpdateTicket")]
    public async Task<IActionResult> UpdateTicket(string id, [FromBody] MongoDBTicketDTO ticketDTO)
    {
        if (ticketDTO == null)
        {
            return BadRequest(new { Message = "Invalid ticket data." });
        }
        
        var existingTicket = await _ticketService.GetTicketByIdAsync(id);

        if (existingTicket == null)
        {
            return NotFound(new { Message = $"MongoDBTicket with ID {id} not found." });
        }
        
        var ticket = new MongoDBTicket
        {
            Price = ticketDTO.Price,
            TicketType = ticketDTO.TicketType,
            CustomerId = ticketDTO.CustomerId,
            FlightrouteId = ticketDTO.FlightrouteId,
            OrderId = ticketDTO.OrderId,
            LuggageIds = ticketDTO.LuggageIds
        };

        await _ticketService.UpdateTicketAsync(id, ticket);
        return Ok(new { Message = "MongoDBTicket updated successfully.", ticket = ticket });
    }

    [HttpDelete("{id}", Name = "DeleteTicket")]
    public async Task<IActionResult> DeleteTicket(string id)
    {
        var existingTicket = await _ticketService.GetTicketByIdAsync(id);

        if (existingTicket == null)
        {
            return NotFound(new { Message = $"MongoDBTicket with ID {id} not found." });
        }

        await _ticketService.DeleteTicketAsync(id);
        return Ok(new { Message = "MongoDBTicket deleted successfully." });
    }
}