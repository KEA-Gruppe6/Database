using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartureController : ControllerBase
{
    [HttpGet(Name = "Departure")]
    public string GetDeparture(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Departure")]
    public string CreateDeparture([FromBody] Departure departure)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Departure")]
    public string UpdateDeparture([FromBody] Departure departure)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Departure")]
    public string DeleteDeparture(int id)
    {
        throw new NotImplementedException();
    }
}