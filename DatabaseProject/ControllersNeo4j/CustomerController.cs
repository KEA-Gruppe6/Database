using Database_project.Neo4j.Entities;
using Database_project.Neo4j.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Neo4jControllers
{
    [Route("api/neo4j/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/<CustomerController>
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<Customer>> GetByTicketId(long ticketId)
        {
            var customers = await _customerService.GetCustomerByTicketIdAsync(ticketId);
            return Ok(customers);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}
