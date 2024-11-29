using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id:long}", Name = "Customer")]
    public async Task<IActionResult> GetCustomer(long id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        var createdCustomer = await _customerService.CreateCustomerAsync(customer);
        return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateCustomer(long id, [FromBody] Customer updatedCustomer)
    {
        if (id != updatedCustomer.CustomerId)
        {
            return BadRequest("Customer ID mismatch.");
        }

        var result = await _customerService.UpdateCustomerAsync(updatedCustomer);
        if (!result)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        return Ok(updatedCustomer);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCustomer(long id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        if (!result)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        return NoContent();
    }
}
