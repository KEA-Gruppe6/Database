using Database_project.Controllers.RequestDTOs;
using Database_project.Core.Entities;
using Database_project.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet(Name = "Order")]
    public string GetOrder(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost("/BuyTickets")]
    public async Task<ActionResult<Order>> BuyTickets([FromBody] OrderRequestDTO tickets)
    {
        try
        {
            var createdTickets = await _orderService.CreateOrder(tickets);

            return Ok(createdTickets);

        }
        catch (Exception)
        {
            return BadRequest("Tickets data is invalid.");
        }
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