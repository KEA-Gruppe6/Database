﻿using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class TicketController : ControllerBase
{
    private ITicketService _ticketService;
    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TicketDTO?>> GetTicket(long id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
        {
            return NotFound($"Ticket with ID {id} not found.");
        }

        return Ok(ticket);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketRequestDTO ticketDTO)
    {
        Ticket ticket = new Ticket
        {
            TicketId = 0,
            Price = ticketDTO.Price,
            TicketTypeId = ticketDTO.TicketTypeId,
            CustomerId = ticketDTO.CustomerId,
            FlightrouteId = ticketDTO.FlightrouteId,
            OrderId = ticketDTO.OrderId,
        };

        try
        {
            var createdTicket = await _ticketService.CreateTicketAsync(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.TicketId }, createdTicket);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<TicketDTO>> UpdateTicket(long id, [FromBody] TicketRequestDTO updatedTicketDTO)
    {
        Ticket updatedTicket = new Ticket
        {
            TicketId = id,
            Price = updatedTicketDTO.Price,
            TicketTypeId = updatedTicketDTO.TicketTypeId,
            CustomerId = updatedTicketDTO.CustomerId,
            FlightrouteId = updatedTicketDTO.FlightrouteId,
            OrderId = updatedTicketDTO.OrderId,
        };

        try
        {
            var result = await _ticketService.UpdateTicketAsync(updatedTicket);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Ticket>> DeleteTicket(long id)
    {
        try
        {
            var result = await _ticketService.DeleteTicketAsync(id);
            return Ok(result);
        }
        catch (DbUpdateException e)
        {
            var errorMessage = e.InnerException?.Message ?? e.Message;

            if (errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"table ""[^""]+\.(?<table>[^""]+)"", column '(?<column>[^']+)'");
                var match = regex.Match(errorMessage);

                if (match.Success)
                {

                    var table = match.Groups["table"].Value;
                    var column = match.Groups["column"].Value;
                    return BadRequest($"Failed to delete. Object linked to column '{column}' in table '{table}'.");
                }
            }

            return BadRequest(errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}