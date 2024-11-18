using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class LuggageController : ControllerBase
{
    [HttpGet(Name = "Luggage")]
    public string GetLuggage(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Luggage")]
    public string CreateLuggage([FromBody] Luggage luggage)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Luggage")]
    public string UpdateLuggage([FromBody] Luggage luggage)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Luggage")]
    public string DeleteLuggage(int id)
    {
        throw new NotImplementedException();
    }
}