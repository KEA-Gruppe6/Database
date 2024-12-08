using Database_project.Controllers.RequestDTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Database_project.Controllers;

[ApiController]
[Route("api/mssql/[controller]")]
public class OrderController : ControllerBase
{
    private IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderDTO?>> GetOrder(long id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] OrderRequestDTO orderDTO)
    {
        Order order = new Order
        {
            OrderId = 0,
            AirlineConfirmationNumber = orderDTO.AirlineConfirmationNumber,
        };

        try
        {
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:long}")]
    public async Task<ActionResult<OrderDTO>> UpdateOrder(long id, [FromBody] OrderRequestDTO updatedOrderDTO)
    {
        Order updatedOrder = new Order
        {
            OrderId = id,
            AirlineConfirmationNumber = updatedOrderDTO.AirlineConfirmationNumber,
        };

        try
        {
            var result = await _orderService.UpdateOrderAsync(updatedOrder);
            return Ok(result);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult<Order>> DeleteOrder(long id)
    {
        try
        {
            var result = await _orderService.DeleteOrderAsync(id);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}