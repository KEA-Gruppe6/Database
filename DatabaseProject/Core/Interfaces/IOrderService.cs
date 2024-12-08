using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.SQL.Interfaces;

public interface IOrderService
{
    Task<OrderDTO?> GetOrderByIdAsync(long id);
    Task<OrderDTO> CreateOrderAsync(Order order);
    Task<OrderDTO> UpdateOrderAsync(Order order);
    Task<Order> DeleteOrderAsync(long id);
}