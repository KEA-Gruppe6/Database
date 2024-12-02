using Database_project.Core;
using Database_project.Core.DTOs;
using Database_project.Core.Entities;
using Database_project.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Services;

public class AirlineService : IAirlineService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public AirlineService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<AirlineDTO?> GetAirlineByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var airline = await context.Airlines
            .Select(a => new AirlineDTO()
            {
                AirlineId = a.AirlineId,
                AirlineName = a.AirlineName,
                Planes = a.Planes.Select(p => new PlaneDTO()
                {
                    PlaneId = p.PlaneId,
                    PlaneDisplayName = p.PlaneDisplayName
                }).ToList()
            })
            .FirstOrDefaultAsync(a => a.AirlineId == id);

        return airline;
    }

    public async Task<Airline> CreateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Ensure that the airline object does not include planes
        airline.Planes = null;

        // Add the airline to the database
        context.Airlines.AddAsync(airline);
        await context.SaveChangesAsync();
        return airline;
    }

    public async Task<AirlineDTO> UpdateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing airline from the database
        var existingAirline = await context.Airlines.FindAsync(airline.AirlineId);
        if (existingAirline == null)
        {
            throw new KeyNotFoundException($"Airline with ID {airline.AirlineId} not found.");
        }

        // Update the properties of the existing airline
        existingAirline.AirlineName = airline.AirlineName;
        // Ensure that the planes collection is not updated
        existingAirline.Planes = existingAirline.Planes;

        // Save the changes to the database
        context.Airlines.Update(existingAirline);
        await context.SaveChangesAsync();

        var returnAirline = GetAirlineByIdAsync(existingAirline.AirlineId).Result;
        return returnAirline;
    }

    public async Task<bool> DeleteAirlineAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();
        try
        {
            var airline = await context.Airlines.Include(a => a.Planes).FirstOrDefaultAsync(a => a.AirlineId == id);
            if (airline == null)
            {
                return false;
            }

            // Remove related planes
            context.Planes.RemoveRange(airline.Planes);

            // Remove the airline
            context.Airlines.Remove(airline);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the airline.", ex);
        }
    }
}