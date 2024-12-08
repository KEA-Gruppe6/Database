using Database_project.Core.MongoDB.RequestDTOs;
using Database_project.Core.MongoDB.Entities;
using Database_project.Core.MongoDB.Services;
using Microsoft.AspNetCore.Mvc;


namespace Database_project.Core.MongoDB.Controllers;

[ApiController]
[Route("api/MongoDB/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    public async Task<IActionResult> GetOrder(string id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found." });
        }

        return Ok(order);
    }

    [HttpGet(Name = "GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }
}