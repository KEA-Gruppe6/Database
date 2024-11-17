using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class AirlineController : ControllerBase
{
    [HttpGet(Name = "Airline")]
    public string GetAirline(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Airline")]
    public string CreateAirline([FromBody] Airline airline)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Airline")]
    public string UpdateAirline([FromBody] Airline airline)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Airline")]
    public string DeleteAirline(int id)
    {
        throw new NotImplementedException();
    }
}