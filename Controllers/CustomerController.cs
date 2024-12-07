using Database_project.Controllers.RequestDTOs;
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
    public async Task<ActionResult<Customer?>> GetCustomer(long id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} not found.");
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerRequestDTO customerDTO)
    {
        Customer customer = new Customer
        {
            CustomerId = 0,
            FirstName = customerDTO.FirstName,
            LastName = customerDTO.LastName,
            PassportNumber = customerDTO.PassportNumber,
        };

        try
        {
            var createdCustomer = await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }
        catch (Exception e)
        {
            if (e is ArgumentException)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(long id, [FromBody] CustomerRequestDTO updatedCustomerDTO)
    {
        Customer updatedCustomer = new Customer
        {
            CustomerId = id,
            FirstName = updatedCustomerDTO.FirstName,
            LastName = updatedCustomerDTO.LastName,
        };

        try
        {
            var result = await _customerService.UpdateCustomerAsync(updatedCustomer);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Customer>> DeleteCustomer(long id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
