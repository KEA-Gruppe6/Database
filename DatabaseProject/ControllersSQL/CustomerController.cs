using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
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
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(customer);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("create-raw-sql")]
    public async Task<ActionResult<Customer>> CreateCustomerRawSQL([FromBody] CustomerRequestDTO customerDTO)
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
            var createdCustomer = await _customerService.CreateCustomerRawSQLAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e) when (e is Microsoft.Data.SqlClient.SqlException || e is InvalidOperationException)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("create-ef-add")]
    public async Task<ActionResult<Customer>> CreateCustomerEFAdd([FromBody] CustomerRequestDTO customerDTO)
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
            var createdCustomer = await _customerService.CreateCustomerEFAddAsync(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
        }
        catch (Exception e) when (e is Microsoft.Data.SqlClient.SqlException || e is InvalidOperationException)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
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
            PassportNumber = updatedCustomerDTO.PassportNumber,
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
        catch (Exception e) when (e is Microsoft.Data.SqlClient.SqlException || e is InvalidOperationException)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
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
        catch (DbUpdateException e)
        {
            var errorMessage = e.InnerException?.Message ?? e.Message;

            if (errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"table ""[^""]+\.(?<table>[^""]+)"", column '(?<column>[^']+)'");
                var match = regex.Match(errorMessage);

                if (match.Success)
                {

                    var table = match.Groups["table"].Value;
                    var column = match.Groups["column"].Value;
                    return BadRequest($"Failed to delete. Object linked to column '{column}' in table '{table}'.");
                }
            }

            return BadRequest(errorMessage);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
