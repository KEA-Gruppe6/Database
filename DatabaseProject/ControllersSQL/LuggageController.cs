﻿using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class LuggageController : ControllerBase
{
    private readonly ILuggageService _luggageService;

    public LuggageController(ILuggageService luggageService)
    {
        _luggageService = luggageService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Luggage?>> GetLuggage(long id)
    {
        var luggage = await _luggageService.GetLuggageByIdAsync(id);
        if (luggage == null)
        {
            return NotFound($"Luggage with ID {id} not found.");
        }

        return Ok(luggage);
    }

    [HttpPost]
    public async Task<ActionResult<Luggage>> CreateLuggage([FromBody] LuggageRequestDTO luggageDTO)
    {
        Luggage luggage = new Luggage
        {
            LuggageId = 0,
            Weight = luggageDTO.MaxWeight,
            IsCarryOn = luggageDTO.IsCarryOn,
            TicketId = luggageDTO.TicketId
        };

        var createdLuggage = await _luggageService.CreateLuggageAsync(luggage);
        return CreatedAtAction(nameof(GetLuggage), new { id = createdLuggage.LuggageId }, createdLuggage);
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<Luggage>> UpdateLuggage(long id, [FromBody] LuggageRequestDTO updatedLuggageDTO)
    {
        Luggage updatedLuggage = new Luggage
        {
            LuggageId = id,
            Weight = updatedLuggageDTO.MaxWeight,
            IsCarryOn = updatedLuggageDTO.IsCarryOn,
            TicketId = updatedLuggageDTO.TicketId
        };

        try
        {
            var result = await _luggageService.UpdateLuggageAsync(updatedLuggage);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Luggage>> DeleteLuggage(long id)
    {
        try
        {
            var result = await _luggageService.DeleteLuggageAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}