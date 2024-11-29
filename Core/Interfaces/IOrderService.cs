using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Controllers.RequestDTOs;

namespace Database_project.Core.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(OrderRequestDTO order);
    Task<bool> UpdateOrderAsync(Order updatedOrder);
    Task<bool> DeleteOrderAsync(int id);
}