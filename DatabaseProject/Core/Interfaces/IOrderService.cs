using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.Interfaces;

public interface IOrderService
{
    Task<OrderDTO?> GetOrderByIdAsync(long id);
    Task<OrderDTO> CreateOrderAsync(Order order);
    Task<OrderDTO> UpdateOrderAsync(Order order);
    Task<Order> DeleteOrderAsync(long id);
}