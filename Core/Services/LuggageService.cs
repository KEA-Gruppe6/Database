using Database_project.Core;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class LuggageService : ILuggageService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public LuggageService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }

        public async Task<Luggage?> GetLuggageByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            var luggage = await context.Luggage.FirstOrDefaultAsync(a => a.LuggageId == id);

            return luggage;
        }

        public async Task<Luggage> CreateLuggageAsync(Luggage luggage)
        {
            await using var context = await _context.CreateDbContextAsync();

            // Check if the ticket exists
            Ticket ticket = await context.Tickets.FindAsync(luggage.TicketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket for Luggage with ID {luggage.LuggageId} found");
            }

            await context.Luggage.AddAsync(luggage);
            await context.SaveChangesAsync();

            return luggage;
        }

        public async Task<Luggage> UpdateLuggageAsync(Luggage Luggage)
        {
            await using var context = await _context.CreateDbContextAsync();

            var existingLuggage = await context.Luggage.FindAsync(Luggage.LuggageId);
            if (existingLuggage == null)
            {
                throw new KeyNotFoundException($"Customer with ID {Luggage.LuggageId} not found.");
            }

            // Check if the ticket exists
            Ticket ticket = await context.Tickets.FindAsync(Luggage.TicketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket for Luggage with ID {Luggage.LuggageId} found");
            }

            // Retrieve and update the existing luggage from the database
            existingLuggage.Weight = Luggage.Weight;
            existingLuggage.IsCarryOn = Luggage.IsCarryOn;
            existingLuggage.TicketId = Luggage.TicketId;

            var returnEntityEntry = context.Luggage.Update(existingLuggage);
            await context.SaveChangesAsync();

            return returnEntityEntry.Entity;
        }

        public async Task<Luggage> DeleteLuggageAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            var luggage = await context.Luggage.FindAsync(id);
            if (luggage == null)
            {
                throw new KeyNotFoundException($"Luggage with ID {id} not found.");
            }
            var returnEntityEntry = context.Luggage.Remove(luggage);
            await context.SaveChangesAsync();

            return returnEntityEntry.Entity;
        }
    }
}