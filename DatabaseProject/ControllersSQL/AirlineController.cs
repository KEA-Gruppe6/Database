﻿using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class AirlineController : ControllerBase
{
    private readonly IAirlineService _airlineService;

    public AirlineController(IAirlineService airlineService)
    {
        _airlineService = airlineService;
    }


    [HttpGet("{id:long}")]
    public async Task<ActionResult<AirlineDTO_Planes?>> GetAirline(long id)
    {
        var airline = await _airlineService.GetAirlineByIdAsync(id);
        if (airline == null)
        {
            return NotFound($"Airline with ID {id} not found.");
        }

        return Ok(airline);
    }


    [HttpPost()]
    public async Task<ActionResult<AirlineDTO_Planes>> CreateAirline([FromBody] AirlineRequestDTO airlineDTO)
    {
        Airline airline = new Airline
        {
            AirlineId = 0,
            AirlineName = airlineDTO.AirlineName,
            Planes = null
        };

        var createdAirline = await _airlineService.CreateAirlineAsync(airline);
        return CreatedAtAction(nameof(GetAirline), new { id = createdAirline.AirlineId }, createdAirline);
    }


    [HttpPatch("{id:long}")]
    public async Task<ActionResult<AirlineDTO_Planes>> UpdateAirline(long id, [FromBody] AirlineRequestDTO updatedAirlineDTO)
    {
        Airline updatedAirline = new Airline
        {
            AirlineId = id,
            AirlineName = updatedAirlineDTO.AirlineName,
        };

        try
        {
            var result = await _airlineService.UpdateAirlineAsync(updatedAirline);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Airline>> DeleteAirline(long id)
    {
        try
        {
            var result = await _airlineService.DeleteAirlineAsync(id);
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