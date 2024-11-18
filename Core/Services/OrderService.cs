using Database_project.Controllers.RequestDTOs;
using Database_project.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class OrderService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public OrderService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }
        public async Task<Order> CreateOrder(OrderRequestDTO buyTicketsRequest)
        {
            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = context.Database.BeginTransaction();

            try
            {
                var departure = await context.Departures
                    .FirstOrDefaultAsync(d => d.DepartureId == buyTicketsRequest.DepartureId);

                if (departure == null)
                {
                    throw new Exception($"Could not find Departure with id: {buyTicketsRequest.DepartureId}");
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
                        Departure = departure,
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
    }
}
