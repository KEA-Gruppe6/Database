using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class PlaneController : ControllerBase
{
    [HttpGet(Name = "Plane")]
    public string GetPlane(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Plane")]
    public string CreatePlane([FromBody] Plane plane)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Plane")]
    public string UpdatePlane([FromBody] Plane plane)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Plane")]
    public string DeletePlane(int id)
    {
        throw new NotImplementedException();
    }
}