using Database_project.Controllers.RequestDTOs;
using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class TicketService : ITicketService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;
        private readonly IFlightrouteService _flightrouteService;

        public TicketService(IDbContextFactory<DatabaseContext> context, IFlightrouteService flightrouteService)
        {
            _context = context;
            _flightrouteService = flightrouteService;
        }

        public async Task<TicketDTO?> GetTicketByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            var ticket = await context.Tickets
                .Include(t => t.TicketType)
                .Include(c => c.Customer)
                .Include(f => f.Flightroute)
                        .ThenInclude(a => a.Plane)

                .Include(f => f.Flightroute)
                        .ThenInclude(a => a.DepartureAirport)

                .Include(f => f.Flightroute)
                        .ThenInclude(a => a.ArrivalAirport)

                .Include(l => l.Luggage)
                .FirstOrDefaultAsync(a => a.TicketId == id);

            var ticketDTO = new TicketDTO()
            {
                TicketId = ticket.TicketId,
                Price = ticket.Price,
                TicketType = new TicketType()
                {
                    TicketTypeId = ticket.TicketType.TicketTypeId,
                    TicketTypeName = ticket.TicketType.TicketTypeName
                },
                Customer = new Customer()
                {
                    CustomerId = ticket.Customer.CustomerId,
                    FirstName = ticket.Customer.FirstName,
                    LastName = ticket.Customer.LastName,
                    PassportNumber = ticket.Customer.PassportNumber
                },
                Flightroute = _flightrouteService.GetFlightrouteByIdAsync(ticket.Flightroute.FlightrouteId).Result,
                Luggage = ticket.Luggage.Select(l => new LuggageDTO_Nested()
                {
                    IsCarryOn = l.IsCarryOn,
                    Weight = l.Weight
                }).ToList()
            };

            return ticketDTO;
        }

        public async Task<TicketDTO> CreateTicketAsync(Ticket ticket)
        {
            await using var context = await _context.CreateDbContextAsync();

            ticket.TicketType = await context.TicketTypes.FindAsync(ticket.TicketTypeId);
            if (ticket.TicketType == null)
            {
                throw new KeyNotFoundException($"Assigned ticket not found");
            }

            ticket.Customer = await context.Customers.FindAsync(ticket.CustomerId);
            if (ticket.Customer == null)
            {
                throw new KeyNotFoundException($"Assigned customer not found");
            }

            ticket.Flightroute = await context.Flightroutes.FindAsync(ticket.FlightrouteId);
            if (ticket.Flightroute == null)
            {
                throw new KeyNotFoundException($"Assigned flightroute not found");
            }

            ticket.Order = await context.Orders.FindAsync(ticket.OrderId);
            if (ticket.Order == null)
            {
                throw new KeyNotFoundException($"Assigned order not found");
            }

            await context.Tickets.AddAsync(ticket);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return GetTicketByIdAsync(ticket.TicketId).Result;
        }
        public async Task<TicketDTO> UpdateTicketAsync(Ticket ticket)
        {
            await using var context = await _context.CreateDbContextAsync();

            // Retrieve the existing ticket from the database
            var existingTicket = await context.Tickets.FindAsync(ticket.TicketId);
            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticket.TicketId} not found.");
            }

            existingTicket.TicketType = ticket.TicketType;
            existingTicket.Price = ticket.Price;
            existingTicket.OrderId = ticket.OrderId;
            existingTicket.CustomerId = ticket.CustomerId;
            existingTicket.FlightrouteId = ticket.FlightrouteId;

            context.Tickets.Update(existingTicket);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return GetTicketByIdAsync(ticket.TicketId).Result;
        }

        public async Task<Ticket> DeleteTicketAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            var ticket = await context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }
            var returnEntityEntry = context.Tickets.Remove(ticket);
            await context.SaveChangesAsync();
            await context.DisposeAsync();

            return returnEntityEntry.Entity;
        }
    }

}
