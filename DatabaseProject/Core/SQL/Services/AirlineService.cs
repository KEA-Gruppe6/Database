using Database_project.Core.SQL;
using Database_project.Core.SQL.DTOs;
using Database_project.Core.SQL.Entities;
using Database_project.Core.SQL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database_project.Core.Services;

public class AirlineService : IAirlineService
{
    private readonly IDbContextFactory<DatabaseContext> _context;

    public AirlineService(IDbContextFactory<DatabaseContext> context)
    {
        _context = context;
    }

    public async Task<AirlineDTO_Planes?> GetAirlineByIdAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        //Get Airline object with nested objects Planes
        var airline = await context.Airlines
            .Include(a => a.Planes)
            .FirstOrDefaultAsync(a => a.AirlineId == id);

        if (airline == null)
        {
            throw new KeyNotFoundException($"Airline with ID {id} not found.");
        }

        //Map Airline object to AirlineDTO object
        var airlineDTO = new AirlineDTO_Planes()
        {
            AirlineId = airline.AirlineId,
            AirlineName = airline.AirlineName,
            Planes = airline.Planes.Select(p => new PlaneDTO()
            {
                PlaneId = p.PlaneId,
                PlaneDisplayName = p.PlaneDisplayName
            }).ToList()
        };
        return airlineDTO;
    }

    public async Task<AirlineDTO_Planes> CreateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Ensure that the airline object does not include planes
        airline.Planes = null;

        // Add the airline to the database
        context.Airlines.AddAsync(airline);
        await context.SaveChangesAsync();

        return GetAirlineByIdAsync(airline.AirlineId).Result;
    }

    public async Task<AirlineDTO_Planes> UpdateAirlineAsync(Airline airline)
    {
        await using var context = await _context.CreateDbContextAsync();

        // Retrieve the existing airline from the database
        var existingAirline = await context.Airlines
            .Include(a => a.Planes)
            .FirstOrDefaultAsync(a => a.AirlineId == airline.AirlineId);

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

        return GetAirlineByIdAsync(airline.AirlineId).Result;
    }

    public async Task<Airline> DeleteAirlineAsync(long id)
    {
        await using var context = await _context.CreateDbContextAsync();

        var airline = await context.Airlines.FindAsync(id);
        if (airline == null)
        {
            throw new KeyNotFoundException($"Airline with ID {id} not found.");
        }
        var returnEntityEntry = context.Airlines.Remove(airline);
        await context.SaveChangesAsync();

        return returnEntityEntry.Entity;
    }
}