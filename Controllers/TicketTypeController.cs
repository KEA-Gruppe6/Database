using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketTypeController : ControllerBase
{
    [HttpGet(Name = "TicketType")]
    public string GetTicketType(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "TicketType")]
    public string CreateTicketType([FromBody] TicketType ticketType)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "TicketType")]
    public string UpdateTicketType([FromBody] TicketType ticketType)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "TicketType")]
    public string DeleteTicketType(int id)
    {
        throw new NotImplementedException();
    }
}