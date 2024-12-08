using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Core.MongoDB.Controllers;


[ApiController]
[Route("api/MongoDB/[controller]")]
public class LuggageController : ControllerBase
{
    private readonly LuggageService _luggageService;

    public LuggageController(LuggageService luggageService)
    {
        _luggageService = luggageService;
    }

    [HttpGet("{id}", Name = "GetLuggage")]
    public async Task<IActionResult> GetLuggage(string id)
    {
        var luggage = await _luggageService.GetLuggageByIdAsync(id);

        if (luggage == null)
        {
            return NotFound(new { Message = $"MongoDBLuggage with ID {id} not found." });
        }

        return Ok(luggage);
    }

    [HttpPost(Name = "CreateLuggage")]
    public async Task<IActionResult> CreateLuggage([FromBody] MongoDBLuggageDTO luggageDTO)
    {
        if (luggageDTO == null)
        {
            return BadRequest(new { Message = "Invalid luggage data." });
        }
        
        var luggage = new MongoDBLuggage
        {
            Weight = luggageDTO.Weight,
            IsCarryOn = luggageDTO.IsCarryOn,
        };

        await _luggageService.CreateLuggageAsync(luggage);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBLuggage created successfully.", luggage = luggage });
    }

    [HttpPatch("{id}", Name = "UpdateLuggage")]
    public async Task<IActionResult> UpdateLuggage(string id, [FromBody] MongoDBLuggageDTO luggageDTO)
    {
        if (luggageDTO == null)
        {
            return BadRequest(new { Message = "Invalid luggage data." });
        }
        
        var existingLuggage = await _luggageService.GetLuggageByIdAsync(id);

        if (existingLuggage == null)
        {
            return NotFound(new { Message = $"MongoDBLuggage with ID {id} not found." });
        }
        
        var luggage = new MongoDBLuggage
        {
            Weight = luggageDTO.Weight,
            IsCarryOn = luggageDTO.IsCarryOn,
        };

        await _luggageService.UpdateLuggageAsync(id, luggage);
        return Ok(new { Message = "MongoDBLuggage updated successfully.", luggage = luggage });
    }

    [HttpDelete("{id}", Name = "DeleteLuggage")]
    public async Task<IActionResult> DeleteLuggage(string id)
    {
        var existingLuggage = await _luggageService.GetLuggageByIdAsync(id);

        if (existingLuggage == null)
        {
            return NotFound(new { Message = $"MongoDBLuggage with ID {id} not found." });
        }

        await _luggageService.DeleteLuggageAsync(id);
        return Ok(new { Message = "MongoDBLuggage deleted successfully." });
    }
}