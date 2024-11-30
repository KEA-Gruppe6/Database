using Database_project.Controllers.RequestDTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public OrderService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Order> CreateOrderAsync(OrderRequestDTO buyTicketsRequest)
        {
            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = context.Database.BeginTransaction();

            try
            {
                var flightroute = await context.Flightroutes
                    .FirstOrDefaultAsync(d => d.FlightrouteId == buyTicketsRequest.FlightrouteId);

                if (flightroute == null)
                {
                    throw new Exception($"Could not find Flightroute with id: {buyTicketsRequest.FlightrouteId}");
                }

                if (buyTicketsRequest.Tickets == null || buyTicketsRequest.Tickets.Count == 0)
                {
                    throw new Exception("Can't buy zero tickets");
                }

                if (buyTicketsRequest.Tickets.Any(t => t.Customer == null))
                {
                    throw new Exception("Ticket must have a customer");
                }

                Order order = new();
                context.Add(order);

                var tickets = new List<Ticket>();

                foreach (var ticket in buyTicketsRequest.Tickets)
                {
                    var newTicket = new Ticket()
                    {
                        Flightroute = flightroute,
                        Price = 100,
                        TicketTypeId = ticket.TicketTypeId,
                        OrderId = order.OrderId
                    };

                    var customer = await context.Customers
                        .FirstOrDefaultAsync(c => ticket.Customer != null && c.PassportNumber == ticket.Customer.PassportNumber);

                    if (customer == null)
                    {
                        customer = new();
                        customer.PassportNumber = ticket.Customer!.PassportNumber;
                        customer.FirstName = ticket.Customer.FirstName;
                        customer.LastName = ticket.Customer.LastName;
                    }

                    newTicket.CustomerId = customer.CustomerId;
                    newTicket.Customer = customer;

                    tickets.Add(newTicket);
                }

                order.AirlineConfirmationNumber = Guid.NewGuid().ToString();
                order.Tickets = tickets;

                await context.Tickets.AddRangeAsync(tickets);
                await context.SaveChangesAsync();
                transaction.Commit();

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                throw new Exception();
            }
        }
        public async Task<bool> UpdateOrderAsync(Order updatedOrder)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            throw new System.NotImplementedException();
        }
    }

}
