using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    [HttpGet(Name = "Ticket")]
    public string GetTicket(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Ticket")]
    public string CreateTicket([FromBody] Ticket ticket)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Ticket")]
    public string UpdateTicket([FromBody] Ticket ticket)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Ticket")]
    public string DeleteTicket(int id)
    {
        throw new NotImplementedException();
    }
}