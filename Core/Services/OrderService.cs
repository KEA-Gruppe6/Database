using Database_project.Controllers.RequestDTOs;
using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;
        private readonly ITicketService _ticketService;

        public OrderService(IDbContextFactory<DatabaseContext> context, ITicketService ticketService)
        {
            _context = context;
            _ticketService = ticketService;
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            var order = await context.Orders
                .Include(m => m.Tickets)
                    .ThenInclude(t => t.TicketType)
                .Include(m => m.Tickets)
                    .ThenInclude(c => c.Customer)
                .Include(m => m.Tickets)
                    .ThenInclude(f => f.Flightroute)
                        .ThenInclude(a => a.Plane)

                .Include(m => m.Tickets)
                    .ThenInclude(f => f.Flightroute)
                        .ThenInclude(a => a.DepartureAirport)

                .Include(m => m.Tickets)
                    .ThenInclude(f => f.Flightroute)
                        .ThenInclude(a => a.ArrivalAirport)

                .Include(m => m.Tickets)
                    .ThenInclude(l => l.Luggage)
                .FirstOrDefaultAsync(a => a.OrderId == id);


            var ticketDTOs = new List<TicketDTO>();
            foreach (var ticket in order.Tickets)
            {
                var ticketDTO = await _ticketService.GetTicketByIdAsync(ticket.TicketId);
                if (ticketDTO != null)
                {
                    ticketDTOs.Add(ticketDTO);
                }
            }

            var orderDTO = new OrderDTO()
            {
                OrderId = order.OrderId,
                AirlineConfirmationNumber = order.AirlineConfirmationNumber,
                Tickets = ticketDTOs
            };

            return orderDTO;
        }

        public async Task<OrderDTO> CreateOrderAsync(Order order)
        {
            await using var context = await _context.CreateDbContextAsync();

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return GetOrderByIdAsync(order.OrderId).Result;
        }
        public async Task<OrderDTO> UpdateOrderAsync(Order order)
        {
            await using var context = await _context.CreateDbContextAsync();

            // Retrieve the existing order from the database
            var existingOrder = await context.Orders.FindAsync(order.OrderId);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {order.OrderId} not found.");
            }

            existingOrder.OrderId = order.OrderId;
            existingOrder.AirlineConfirmationNumber = order.AirlineConfirmationNumber;

            context.Orders.Update(existingOrder);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return GetOrderByIdAsync(order.OrderId).Result;
        }

        public async Task<Order> DeleteOrderAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            var order = await context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
            var returnEntityEntry = context.Orders.Remove(order);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return returnEntityEntry.Entity;
        }
    }

}
