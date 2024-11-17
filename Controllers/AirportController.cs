using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class AirportController : ControllerBase
{
    [HttpGet(Name = "Airport")]
    public string GetAirport(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Airport")]
    public string CreateAirport([FromBody] Airport airport)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Airport")]
    public string UpdateAirport([FromBody] Airport airport)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Airport")]
    public string DeleteAirport(int id)
    {
        throw new NotImplementedException();
    }
}