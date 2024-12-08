using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Database_project.Core.MongoDB.DTO;
using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("api/MongoDB/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}", Name = "GetCustomer")]
    public async Task<IActionResult> GetCustomer(string id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            return NotFound(new { Message = $"MongoDBCustomer with ID {id} not found." });
        }

        return Ok(customer);
    }

    [HttpPost(Name = "CreateCustomer")]
    public async Task<IActionResult> CreateCustomer([FromBody] MongoDBCustomerDTO customerDTO)
    {
        if (customerDTO == null)
        {
            return BadRequest(new { Message = "Invalid customer data." });
        }
        
        var customer = new MongoDBCustomer
        {
            FirstName = customerDTO.FirstName,
            LastName = customerDTO.LastName,
            PassportNumber = customerDTO.PassportNumber,
        };

        await _customerService.InsertCustomerAsync(customer);
        return StatusCode((int)HttpStatusCode.Created, new { Message = "MongoDBCustomer created successfully.", Customer = customer });
    }

    [HttpPatch("{id}", Name = "UpdateCustomer")]
    public async Task<IActionResult> UpdateCustomer(string id, [FromBody] MongoDBCustomerDTO customerDTO)
    {
        if (customerDTO == null)
        {
            return BadRequest(new { Message = "Invalid customer data." });
        }
        
        var existingCustomer = await _customerService.GetCustomerByIdAsync(id);

        if (existingCustomer == null)
        {
            return NotFound(new { Message = $"MongoDBCustomer with ID {id} not found." });
        }
        
        var customer = new MongoDBCustomer
        {
            FirstName = customerDTO.FirstName,
            LastName = customerDTO.LastName,
            PassportNumber = customerDTO.PassportNumber,
        };

        await _customerService.UpdateCustomerAsync(id, customer);
        return Ok(new { Message = "MongoDBCustomer updated successfully.", Customer = customer });
    }

    [HttpDelete("{id}", Name = "DeleteCustomer")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        var existingCustomer = await _customerService.GetCustomerByIdAsync(id);

        if (existingCustomer == null)
        {
            return NotFound(new { Message = $"MongoDBCustomer with ID {id} not found." });
        }

        await _customerService.DeleteCustomerAsync(id);
        return Ok(new { Message = "MongoDBCustomer deleted successfully." });
    }
}
