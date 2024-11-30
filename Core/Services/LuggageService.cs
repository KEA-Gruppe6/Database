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
            if (luggage.TicketId != null)
            {
                Ticket ticket = await context.Tickets.FindAsync(luggage.TicketId);
                if (ticket == null)
                {
                    throw new KeyNotFoundException("Ticket not found");
                }
            }

            await context.Luggage.AddAsync(luggage);
            await context.SaveChangesAsync();

            return luggage;
        }

        public async Task<bool> UpdateLuggageAsync(Luggage updatedLuggage)
        {
            await using var context = await _context.CreateDbContextAsync();

            // Check if the ticket exists
            if (updatedLuggage.TicketId != null)
            {
                Ticket ticket = await context.Tickets.FindAsync(updatedLuggage.TicketId);
                if (ticket == null)
                {
                    throw new KeyNotFoundException("Ticket not found");
                }
            }

            // Retrieve and update the existing luggage from the database
            try
            {
                var existingLuggage = await context.Luggage
                    .FirstOrDefaultAsync(a => a.LuggageId == updatedLuggage.LuggageId);

                if (existingLuggage == null)
                {
                    return false;  // If the updatedLuggage doesn't exist, return false
                }

                existingLuggage.MaxWeight = updatedLuggage.MaxWeight;
                existingLuggage.IsCarryOn = updatedLuggage.IsCarryOn;
                existingLuggage.TicketId = updatedLuggage.TicketId;

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteLuggageAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var luggage = await context.Luggage.FindAsync(id);
                if (luggage == null)
                {
                    return false;
                }

                context.Luggage.Remove(luggage);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}