using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class LuggageController : ControllerBase
{
    private readonly ILuggageService _luggageService;

    public LuggageController(ILuggageService luggageService)
    {
        _luggageService = luggageService;
    }

    [HttpGet("{id:long}", Name = "Luggage")]
    public async Task<IActionResult> GetLuggage(long id)
    {
        var luggage = await _luggageService.GetLuggageByIdAsync(id);
        if (luggage == null)
        {
            return NotFound($"Luggage with ID {id} not found.");
        }

        return Ok(luggage);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLuggage([FromBody] Luggage luggage)
    {
        Luggage createdLuggage;
        try
        {
            createdLuggage = await _luggageService.CreateLuggageAsync(luggage);
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }
        return CreatedAtAction(nameof(GetLuggage), new { id = createdLuggage.LuggageId }, createdLuggage);
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateLuggage(long id, [FromBody] Luggage luggage)
    {
        if (id != luggage.LuggageId)
        {
            return BadRequest("Luggage ID mismatch.");
        }
        try
        {
            bool result = await _luggageService.UpdateLuggageAsync(luggage);
            if (!result)
            {
                return NotFound($"Luggage with ID {id} not found.");
            }
        }
        catch (KeyNotFoundException e)
        {
            return BadRequest(e.Message);
        }

        return Ok(luggage);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteLuggage(long id)
    {
        var result = await _luggageService.DeleteLuggageAsync(id);
        if (!result)
        {
            return NotFound($"Luggage with ID {id} not found.");
        }

        return NoContent();
    }
}