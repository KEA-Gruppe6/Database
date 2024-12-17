using Database_project.Core.SQL;
using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
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

        public async Task<FlightrouteDTO?> GetFlightrouteByIdAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            //Get Flightroute object with nested objects DepartureAirport and ArrivalAirport
            var flightroute = await context.Flightroutes
                .Include(m => m.ArrivalAirport)
                .Include(m => m.DepartureAirport)
                .Include(m => m.Plane)
                .FirstOrDefaultAsync(a => a.FlightrouteId == id);

            if (flightroute == null)
            {
                return null;
            }

            //Map flightroute object to FlightrouteDTO object
            var flightrouteDTO = new FlightrouteDTO()
            {
                FlightrouteId = flightroute.FlightrouteId,
                DepartureTime = flightroute.DepartureTime,
                ArrivalTime = flightroute.ArrivalTime,
                ArrivalAirport = flightroute.ArrivalAirport != null ? new Airport()
                {
                    AirportId = flightroute.ArrivalAirport.AirportId,
                    AirportName = flightroute.ArrivalAirport.AirportName,
                    AirportCity = flightroute.ArrivalAirport.AirportCity,
                    Municipality = flightroute.ArrivalAirport.Municipality,
                    AirportAbbreviation = flightroute.ArrivalAirport.AirportAbbreviation
                } : null,
                DepartureAirport = flightroute.DepartureAirport != null ? new Airport()
                {
                    AirportId = flightroute.DepartureAirport.AirportId,
                    AirportName = flightroute.DepartureAirport.AirportName,
                    AirportCity = flightroute.DepartureAirport.AirportCity,
                    Municipality = flightroute.DepartureAirport.Municipality,
                    AirportAbbreviation = flightroute.DepartureAirport.AirportAbbreviation
                } : null,
                Plane = flightroute.Plane != null ? new PlaneDTO()
                {
                    PlaneId = flightroute.Plane.PlaneId,
                    PlaneDisplayName = flightroute.Plane.PlaneDisplayName,
                } : null
            };
            return flightrouteDTO;
        }

        public async Task<FlightrouteDTO> CreateFlightrouteAsync(Flightroute flightroute)
        {
            await using var context = await _context.CreateDbContextAsync();

            flightroute.DepartureAirport = await context.Airports.FindAsync(flightroute.DepartureAirportId);
            flightroute.ArrivalAirport = await context.Airports.FindAsync(flightroute.ArrivalAirportId);

            if (flightroute.DepartureAirport == null || flightroute.ArrivalAirport == null)
            {
                throw new KeyNotFoundException("One or both airports not found");
            }

            flightroute.Plane = await context.Planes.FindAsync(flightroute.PlaneId);
            if (flightroute.Plane == null)
            {
                throw new KeyNotFoundException("Plane not found");
            }

            await context.Flightroutes.AddAsync(flightroute);
            await context.SaveChangesAsync();

            return GetFlightrouteByIdAsync(flightroute.FlightrouteId).Result;
        }

        public async Task<FlightrouteDTO> UpdateFlightrouteAsync(Flightroute flightroute)
        {
            await using var context = await _context.CreateDbContextAsync();

            // Retrieve the existing flightroute from the database
            var existingFlightroute = await context.Flightroutes.FindAsync(flightroute.FlightrouteId);
            if (existingFlightroute == null)
            {
                throw new KeyNotFoundException($"Flightroute with ID {flightroute.FlightrouteId} not found.");
            }

            // Check if the airports exist
            flightroute.DepartureAirport = await context.Airports.FindAsync(flightroute.DepartureAirportId);
            flightroute.ArrivalAirport = await context.Airports.FindAsync(flightroute.ArrivalAirportId);
            if (flightroute.DepartureAirport == null || flightroute.ArrivalAirport == null)
            {
                throw new KeyNotFoundException("One or both airports not found");
            }

            flightroute.Plane = await context.Planes.FindAsync(flightroute.PlaneId);
            if (flightroute.Plane == null)
            {
                throw new KeyNotFoundException("Plane not found");
            }

            existingFlightroute.DepartureTime = flightroute.DepartureTime;
            existingFlightroute.ArrivalTime = flightroute.ArrivalTime;
            existingFlightroute.DepartureAirportId = flightroute.DepartureAirportId;
            existingFlightroute.ArrivalAirportId = flightroute.ArrivalAirportId;
            existingFlightroute.PlaneId = flightroute.PlaneId;

            context.Flightroutes.Update(existingFlightroute);
            await context.SaveChangesAsync();

            return GetFlightrouteByIdAsync(flightroute.FlightrouteId).Result;

        }

        public async Task<Flightroute> DeleteFlightrouteAsync(long id)
        {
            await using var context = await _context.CreateDbContextAsync();

            var flightroute = await context.Flightroutes.FindAsync(id);
            if (flightroute == null)
            {
                throw new KeyNotFoundException($"Flightroute with ID {id} not found.");
            }
            var returnEntityEntry = context.Flightroutes.Remove(flightroute);
            await context.SaveChangesAsync();

            return returnEntityEntry.Entity;
        }
    }
}