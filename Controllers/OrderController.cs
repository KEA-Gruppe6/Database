using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpGet(Name = "Order")]
    public string GetOrder(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Order")]
    public string CreateOrder([FromBody] Order order)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Order")]
    public string UpdateOrder([FromBody] Order order)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Order")]
    public string DeleteOrder(int id)
    {
        throw new NotImplementedException();
    }
}