using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class MaintenanceController : ControllerBase
{
    [HttpGet(Name = "Maintenance")]
    public string GetMaintenance(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Maintenance")]
    public string CreateMaintenance([FromBody] Maintenance maintenance)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Maintenance")]
    public string UpdateMaintenance([FromBody] Maintenance maintenance)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Maintenance")]
    public string DeleteMaintenance(int id)
    {
        throw new NotImplementedException();
    }
}