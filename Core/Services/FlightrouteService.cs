using Database_project.Core;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services
{
    public class FlightrouteService : IFlightrouteService
    {
        private readonly IDbContextFactory<DatabaseContext> _context;

        public FlightrouteService(IDbContextFactory<DatabaseContext> context)
        {
            _context = context;
        }

        public async Task<Flightroute?> GetFlightrouteByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            var flightroute = await context.Flightroutes.FirstOrDefaultAsync(a => a.FlightrouteId == id);

            return flightroute;
        }

        public async Task<Flightroute> CreateFlightrouteAsync(Flightroute flightroute)
        {
            await using var context = await _context.CreateDbContextAsync();

            flightroute.DepartureAirport = await context.Airports.FindAsync(flightroute.DepartureAirportId);
            flightroute.ArrivalAirport = await context.Airports.FindAsync(flightroute.ArrivalAirportId);

            if (flightroute.DepartureAirport == null || flightroute.ArrivalAirport == null)
            {
                throw new KeyNotFoundException("One or both airports not found");
            }

            await context.Flightroutes.AddAsync(flightroute);
            await context.SaveChangesAsync();

            return flightroute;
        }

        public async Task<bool> UpdateFlightrouteAsync(Flightroute updatedFlightroute)
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var existingFlightroute = await context.Flightroutes
                    .FirstOrDefaultAsync(a => a.FlightrouteId == updatedFlightroute.FlightrouteId);

                if (existingFlightroute == null)
                {
                    return false;  // If the updatedFlightroute doesn't exist, return false
                }

                existingFlightroute.DepartureTime = updatedFlightroute.DepartureTime;
                existingFlightroute.ArrivalTime = updatedFlightroute.ArrivalTime;
                existingFlightroute.DepartureAirportId = updatedFlightroute.DepartureAirportId;
                existingFlightroute.ArrivalAirportId = updatedFlightroute.ArrivalAirportId;

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> DeleteFlightrouteAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var flightroute = await context.Flightroutes.FindAsync(id);
                if (flightroute == null)
                {
                    return false;
                }

                context.Flightroutes.Remove(flightroute);
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