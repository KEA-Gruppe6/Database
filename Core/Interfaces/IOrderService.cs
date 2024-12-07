using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(long id);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<Order> DeleteOrderAsync(long id);
}