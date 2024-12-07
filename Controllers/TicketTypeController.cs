using Database_project.Controllers.RequestDTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketTypeController(ITicketTypeService ticketTypeService) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<List<TicketType>>> GetAllTicketTypes()
    {
        try
        {
            return Ok(await ticketTypeService.GetTicketTypesAsync());
        }
        catch (Exception e)
        {
            return Problem(type: e.GetType().ToString(), title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TicketType>> GetTicketType(long id)
    {
        try
        {
            return Ok(await ticketTypeService.GetTicketTypeByIdAsync(id));
        }
        catch (ArgumentException a)
        {
            return BadRequest(a.Message);
        }
        catch (Exception e)
        {
            return Problem(type: e.GetType().ToString(), title: e.Message, detail: e.StackTrace);
        }
    }

    [HttpPost()]
    public async Task<ActionResult<TicketType>> CreateTicketType([FromBody] TicketTypeDTO ticketTypeDTO)
    {
        var newTicketType = new TicketType { TicketTypeId = 0, TicketTypeName = ticketTypeDTO.TicketTypeName };
        
        try
        {
            var createdTicketType = ticketTypeService.CreateTicketTypeAsync(newTicketType);
            return CreatedAtAction(nameof(GetTicketType), new { id = createdTicketType.Result.TicketTypeId }, createdTicketType.Result);
        }
        catch (Exception e)
        {
            return Problem(type: e.GetType().ToString(), title: e.Message, detail: e.StackTrace);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<TicketType>> UpdateTicketType(long id, [FromBody] TicketTypeDTO ticketTypeDTO)
    {
        var updatedTicketType = new TicketType {TicketTypeId = id, TicketTypeName = ticketTypeDTO.TicketTypeName};

        try
        {
            return Ok(await ticketTypeService.UpdateTicketTypeAsync(updatedTicketType));
        }
        catch (ArgumentException a)
        {
            return BadRequest(a.Message);
        }
        catch (Exception e)
        {
            return Problem(type: e.GetType().ToString(), title: e.Message, detail: e.StackTrace);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<TicketType>> DeleteTicketType(long id)
    {
        try
        {
            return Ok(await ticketTypeService.DeleteTicketTypeAsync(id));
        }
        catch (ArgumentException a)
        {
            return BadRequest(a.Message);
        }
        catch (Exception e)
        {
            return Problem(type: e.GetType().ToString(), title: e.Message, detail: e.StackTrace);
        }
    }
}