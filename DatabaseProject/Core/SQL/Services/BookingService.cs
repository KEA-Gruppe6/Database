using Database_project.Core.SQL;
using Database_project.Core.SQL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class BookingService //: IBookingService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public BookingService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }

        public class OrderRequestDTO
        {
            public long FlightrouteId { get; set; }
            public List<SingleTicketDTO>? Tickets { get; set; }
        }

        public class SingleTicketDTO
        {
            public long TicketTypeId { get; set; }
            public Customer? Customer { get; set; }
        }

        public async Task<Order> CreateBooking(OrderRequestDTO buyTicketsRequest)
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
    }
}
