using Database_project.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    [HttpGet(Name = "Customer")]
    public string GetCustomer(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "Customer")]
    public string CreateCustomer([FromBody] Customer customer)
    {
        throw new NotImplementedException();
    }

    [HttpPatch(Name = "Customer")]
    public string UpdateCustomer([FromBody] Customer customer)
    {
        throw new NotImplementedException();
    }

    [HttpDelete(Name = "Customer")]
    public string DeleteCustomer(int id)
    {
        throw new NotImplementedException();
    }
}
